[<RequireQualifiedAccess>]
module SuperClip.Forms.Session.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module WebSocketProxy = Dap.Remote.WebSocketProxy.Proxy

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.Session.Types
open SuperClip.Forms.Session.Tasks
module PrimaryService = SuperClip.Core.Primary.Service
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

type ActorOperate = Operate<Agent, Model, Msg>

let mutable private cloudPeers : Peers option = None

let private doSetItemToCloud (item : Item) : ActorOperate =
    fun runner (model, cmd) ->
        let shouldNotSet =
            model.Auth.IsNone
            || not model.Syncing
            || item.IsEmpty || (
                match model.LastCloudItem with
                | None ->
                    false
                | Some lastItem ->
                    lastItem = item
                    || lastItem.Content.Hash = item.Content.Hash
            )
        if not shouldNotSet then
            model.LastCloudItem <- Some item
            (model.Auth, model.Channel)
            ||> Option.iter2 (fun auth channel ->
                let item = item.ForCloud auth.Self auth.CryptoKey
                runner.Actor.Args.Stub.Post <| CloudTypes.DoSetItem item
            )
        (model, cmd)

let private doAddHistory (runner : Agent) (item : Item) =
    if not item.IsEmpty then
        runner.Actor.Args.History.Actor.Handle <| HistoryTypes.DoAdd item None
    item

let private onPrimaryEvt (evt : Clipboard.Evt) : ActorOperate =
    fun runner (model, cmd) ->
        logWarn runner "Session" "PrimaryEvt" evt
        match evt with
        | Clipboard.OnSet item ->
            item
        | Clipboard.OnChanged item ->
            item
        |> doAddHistory runner
        |> doSetItemToCloud
        <| runner <| (model, cmd)

let private onStubEvt (evt : CloudTypes.Evt) : ActorOperate =
    fun runner (model, cmd) ->
        logWarn runner "Session" "CloudEvt" evt
        if model.Auth.IsSome && model.Syncing then
            match evt with
            | CloudTypes.OnItemChanged item ->
                let auth = model.Auth |> Option.get
                item.Decrypt runner auth.CryptoKey
                |> doAddHistory runner
                |> (fun item ->
                    model.LastCloudItem <- Some item
                    runner.Actor.Args.Primary.Post <| Clipboard.DoSet item.Content None
                )
            | _ -> ()
        (model, cmd)

let private onStubRes (res : CloudTypes.ClientRes) : ActorOperate =
    fun runner (model, cmd) ->
        logWarn runner "Session" "CloudRes" res
        match res with
        | CloudTypes.OnJoin (_req, (Ok token)) ->
            runner.Actor.Args.Stub.Post <| CloudTypes.DoAuth token
            let auth =
                model.Auth
                |> Option.map (fun auth ->
                    let auth = {auth with Token = token.Value}
                    runner.Actor.Args.Pref.Properties.Credential.SetValue <| Some auth
                    |> ignore
                    auth
                )
            updateModel (fun m -> {m with Auth = auth})
            |-|- addSubCmd Evt ^<| OnAuthChanged auth
            |-|- addSubCmd Evt ^<| OnJoinSucceed token
        | CloudTypes.OnJoin (_req, (Error err)) ->
            updateModel (fun m -> {m with Auth = None})
            |-|- addSubCmd Evt ^<| OnJoinFailed err
        | CloudTypes.OnAuth (_req, (Ok peers)) ->
            runner.AddTask ignoreOnFailed <| doSetChannelAsync peers
            noOperation
        | CloudTypes.OnAuth (_req, (Error err)) ->
            addSubCmd Evt ^<| OnAuthFailed err
        | _ ->
            noOperation
        <| runner <| (model, cmd)

let private doSetAuth req ((auth, callback) : Pref.Credential * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        let joinReq = Join.Req.Create auth.Self auth.PassHash
        runner.Actor.Args.Stub.Post <| CloudTypes.DoJoin joinReq
        reply runner callback <| ack req ()
        (runner, model, cmd)
        |=|> updateModel (fun m -> {m with Auth = Some auth})

let private doResetAuth req (callback : Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        model.Auth
        |> Option.bind (fun a -> if a.Token <> "" then Some a.Token else None)
        |> Option.iter (fun token ->
            runner.Actor.Args.Stub.Post <| CloudTypes.DoLeave (JsonString token)
            reply runner callback <| ack req ()
        )
        runner.Actor.Args.Pref.Properties.Credential.SetValue None
        |> ignore
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with Auth = None})
        |=|> addSubCmd Evt ^<| OnAuthChanged None

let private doSetSyncing req ((syncing, callback) : bool * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        reply runner callback <| ack req ()
        if syncing <> model.Syncing then
            (runner, model, cmd)
            |-|> updateModel (fun m -> {m with Syncing = syncing})
            |=|> addSubCmd Evt ^<| OnSyncingChanged syncing
        else
            (model, cmd)

let private onStubStatus (status : LinkStatus) : ActorOperate =
    fun runner (model, cmd) ->
        logWarn runner "Session" "onStubStatus" status
        match status with
        | LinkStatus.Linked ->
            runner.Actor.Args.Pref.Properties.Credential.Value
            |> Option.map (fun auth ->
                addSubCmd Req ^<| DoSetAuth (auth, None)
            )
        | LinkStatus.Closed ->
            Some <| updateModel (fun _ -> Model.Empty)
        | _ ->
            None
        |> Option.defaultValue noOperation
        <| runner <| (model, cmd)

let private init : Init<IAgent<Msg>, Args, Model, Msg> =
    fun initer args ->
        (Model.Empty, noCmd)

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
        match msg with
        | Req req ->
            match req with
            | DoSetAuth (a, b) -> doSetAuth req (a, b)
            | DoResetAuth (a) -> doResetAuth req (a)
            | DoSetSyncing (a, b) -> doSetSyncing req (a, b)
        | Evt _evt -> noOperation
        | PrimaryEvt evt -> onPrimaryEvt evt
        | StubRes res -> onStubRes res
        | StubEvt evt -> onStubEvt evt
        | StubStatus status -> onStubStatus status
        | InternalEvt evt ->
            match evt with
            | SetChannel (peers, channel) ->
                updateModel (fun m -> {m with Channel = Some channel})
                |-|- addSubCmd Evt ^<| OnAuthSucceed peers
        <| runner <| (model, [])

let private subscribe : Subscribe<Agent, Model, Msg> =
    fun runner model ->
        Cmd.batch [
            subscribeBus runner model PrimaryEvt runner.Actor.Args.Primary.Actor.OnEvent
            subscribeBus runner model StubRes runner.Actor.Args.Stub.OnResponse
            subscribeBus runner model StubEvt runner.Actor.Args.Stub.Actor.OnEvent
            subscribeBus runner model StubStatus runner.Actor.Args.Stub.OnStatus
        ]

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)
    |> fun s -> s.WithSubscribe subscribe


