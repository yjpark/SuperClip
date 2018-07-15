module SuperClip.Core.Primary.Logic

open NodaTime
open Elmish
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module TickerTypes = Dap.Platform.Ticker.Types
module TickerService = Dap.Platform.Ticker.Service

type ActorOperate = Operate<Agent, Model, Msg>

let private doGet req (callback : Callback<Clipboard.Item>) : ActorOperate =
    fun runner (model, cmd) ->
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with WaitingCallbacks = (req, callback) :: m.WaitingCallbacks})
        |=|> match model.Getting with
                | true ->
                    noOperation
                | false ->
                    runner.AddTask onGetFailed doGetAsync
                    updateModel (fun m -> {m with Getting = true})

let private doSet req ((content, callback) : Clipboard.Content * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        match content with
        | Text text -> CrossClipboard.Current.SetText (text)
        let current =
            {
                Time = runner.Clock.Now
                Index = model.Current.Index + 1
                Content = content
            }
        runner.Deliver <| Evt ^<| OnChanged current
        reply runner callback <| ack req ()
        ({model with Current = current}, cmd)

let private doInit : ActorOperate =
    fun runner (model, cmd) ->
        let ticker = runner.Env |> TickerService.get noKey
        ticker.Actor.OnEvent.AddWatcher runner "OnTick" (fun evt ->
            match evt with
            | TickerTypes.OnTick (a, b) ->
                runner.Deliver <| InternalEvt ^<| OnTick (a, b)
            | _ -> ()
        )
        (model, cmd)

let private onTick ((time, _duration) : Instant * Duration) : ActorOperate =
    fun runner (model, cmd) ->
        if not model.Getting && time >= model.NextGetTime then
            runner.AddTask onGetFailed doGetAsync
            ({model with Getting = true}, cmd)
        else
            (model, cmd)

let private onGet (res : Result<Clipboard.Content, exn>) : ActorOperate =
    fun runner (model, cmd) ->
        let current =
            match res with
            | Ok content ->
                let current =
                    {
                        Time = runner.Clock.Now
                        Index = model.Current.Index + 1
                        Content = content
                    }
                Some current
            | Error _err ->
                None
        runner.AddTask ignoreOnFailed <| onGetAsync res current model.WaitingCallbacks
        let current = current |> Option.defaultValue model.Current
        let nextGetTime =
            runner.Actor.Args.CheckInterval
            |> Option.map (fun i ->
                runner.Clock.Now + Duration.FromSeconds (float i)
            )|> Option.defaultValue model.NextGetTime
        (runner, model, cmd)
        |=|> updateModel (fun m ->
            {model with
                Current = current
                Getting = false
                NextGetTime = nextGetTime
                WaitingCallbacks = []
            }
        )

let private update : Update<Agent, Model, Msg> =
    fun runner model msg ->
        match msg with
        | InternalEvt evt ->
            match evt with
            | DoInit -> doInit
            | OnTick (a, b) -> onTick (a, b)
            | OnGet a -> onGet a
        | Req req ->
            match req with
            | DoGet a -> doGet req a
            | DoSet (a, b) -> doSet req (a, b)
        | Evt _evt -> noOperation
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    fun runner args ->
        ({
            Current = Clipboard.Item.Empty
            Getting = false
            NextGetTime = Instant.MinValue
            WaitingCallbacks = []
        }, Cmd.ofMsg <| InternalEvt DoInit)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)

