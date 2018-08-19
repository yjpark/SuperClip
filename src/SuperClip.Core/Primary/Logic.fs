module SuperClip.Core.Primary.Logic

open NodaTime
open Elmish
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform
open Dap.Forms

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module TickerTypes = Dap.Platform.Ticker.Types
module TickerService = Dap.Platform.Ticker.Service

type ActorOperate = Operate<Agent, Model, Msg>

let private doGet' : ActorOperate =
    fun runner (model, cmd) ->
        let index = model.GettingIndex + 1
        runner.AddUiTask onGetFailed <| doGetAsync index
        ({model with
            Getting = true
            GettingIndex = index
            TimeoutTime = runner.Clock.Now + runner.Actor.Args.TimeoutDuration
        }, cmd)

let private doGet req (callback : Callback<Item>) : ActorOperate =
    fun runner (model, cmd) ->
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with WaitingCallbacks = (req, callback) :: m.WaitingCallbacks})
        |=|> match model.Getting with
                | true ->
                    noOperation
                | false ->
                    doGet'

let private doSet req ((content, callback) : Content * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        match content with
        | Text text ->
            runner.RunUiFunc (fun _ -> CrossClipboard.Current.SetText (text))
            |> ignore
        let current = Item.Create runner.Clock.Now Local content
        runner.Deliver <| Evt ^<| OnSet current
        reply runner callback <| ack req ()
        ({model with Current = current}, cmd)

let private doInit : ActorOperate =
    fun runner (model, cmd) ->
        let ticker = runner.Env |> TickerService.get NoKey
        ticker.Actor.OnEvent.AddWatcher runner "OnTick" (fun evt ->
            match evt with
            | TickerTypes.OnTick (a, b) ->
                runner.Deliver <| InternalEvt ^<| OnTick (a, b)
            | _ -> ()
        )
        (model, cmd)

let private onTick ((time, _duration) : Instant * Duration) : ActorOperate =
    fun runner (model, cmd) ->
        if (not model.Getting && time >= model.NextGetTime)
            || (model.Getting && time >= model.TimeoutTime) then
            (runner, model, cmd)
            |=|> doGet'
        else
            (model, cmd)

let private onGet (res : Result<Content, exn>) : ActorOperate =
    fun runner (model, cmd) ->
        let (current, changed) =
            match res with
            | Ok content ->
                if content <> model.Current.Content then
                    (Item.Create runner.Clock.Now Local content, true)
                else
                    (model.Current, false)
            | Error _err ->
                (model.Current, false)
        runner.AddTask ignoreOnFailed <| onGetAsync res current model.WaitingCallbacks
        let nextGetTime =
            runner.Actor.Args.CheckInterval
            |> Option.map (fun i ->
                runner.Clock.Now + Duration.FromSeconds (float i)
            )|> Option.defaultValue model.NextGetTime
        (runner, model, cmd)
        |-|> updateModel (fun m ->
            {model with
                Current = current
                Getting = false
                NextGetTime = nextGetTime
                WaitingCallbacks = []
            }
        )|=|> if changed then
                addSubCmd Evt <| OnChanged current
            else
                noOperation

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
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
            Current = Item.Empty
            Getting = false
            GettingIndex = -1
            NextGetTime = Instant.MinValue
            TimeoutTime = Instant.MaxValue
            WaitingCallbacks = []
        }, Cmd.ofMsg <| InternalEvt DoInit)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)

