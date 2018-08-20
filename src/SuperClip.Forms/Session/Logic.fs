[<RequireQualifiedAccess>]
module SuperClip.Forms.Session.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Local.App
open Dap.Prelude
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
            match model.LastCloudItem with
            | None ->
                false
            | Some lastItem ->
                lastItem = item
                || lastItem.Content.Hash = item.Content.Hash
        if not shouldNotSet then
            model.LastCloudItem <- Some item
            (model.Auth, model.Self, model.Channel)
            |||> Option.iter3 (fun auth self channel ->
                let item = item.ForCloud self auth.CryptoKey
                runner.Actor.Args.Stub.Post <| CloudTypes.DoSetItem item
            )
        (model, cmd)

let private doAddHistory (runner : Agent) item =
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
                    {auth with Token = token.Value}
                )
            updateModel (fun m -> {m with Auth = auth})
            |-|- addSubCmd Evt ^<| OnJoinSucceed token
        | CloudTypes.OnJoin (_req, (Error err)) ->
            addSubCmd Evt ^<| OnJoinFailed err
        | CloudTypes.OnAuth (_req, (Ok peers)) ->
            runner.AddTask ignoreOnFailed <| doSetChannelAsync peers
            noOperation
        | CloudTypes.OnAuth (_req, (Error err)) ->
            addSubCmd Evt ^<| OnAuthFailed err
        | _ ->
            noOperation
        <| runner <| (model, cmd)

let private doInit : ActorOperate =
    fun runner (model, cmd) ->
        let password = "test"
        let auth : Pref.Credential =
            {
                Device = Device.New "test"
                Channel = Channel.CreateWithName "test"
                PassHash = calcPassHash password
                CryptoKey = calcCryptoKey password
                Token = ""
            }
        (runner, model, cmd)
        |=|> addSubCmd Req ^<| DoSetAuth (auth, None)

let private doSetAuth req ((auth, callback) : Pref.Credential * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        let channel = auth.Channel
        let self = Peer.Create channel auth.Device
        let joinReq = Join.Req.Create self auth.PassHash
        runner.Actor.Args.Stub.Post <| CloudTypes.DoJoin joinReq
        reply runner callback <| ack req ()
        (runner, model, cmd)
        |=|> updateModel (fun m ->
            {m with
                Auth = Some auth
                Self = Some self
            }
        )

let private init : Init<IAgent<Msg>, Args, Model, Msg> =
    fun initer args ->
        let model =
            {
                Auth = None
                Self = None
                Channel = None
                LastCloudItem = None
            }
        (initer, model, [])
        |=|> addSubCmd InternalEvt DoInit

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
        match msg with
        | Req req ->
            match req with
            | DoSetAuth (a, b) -> doSetAuth req (a, b)
        | Evt _evt -> noOperation
        | PrimaryEvt evt -> onPrimaryEvt evt
        | StubRes res -> onStubRes res
        | StubEvt evt -> onStubEvt evt
        | InternalEvt evt ->
            match evt with
            | DoInit ->
                doInit
            | SetChannel channel ->
                updateModel (fun m -> {m with Channel = Some channel})
        <| runner <| (model, [])

let private subscribe : Subscribe<Agent, Model, Msg> =
    fun runner model ->
        Cmd.batch [
            subscribeBus runner model PrimaryEvt runner.Actor.Args.Primary.Actor.OnEvent
            subscribeBus runner model StubRes runner.Actor.Args.Stub.OnResponse
            subscribeBus runner model StubEvt runner.Actor.Args.Stub.Actor.OnEvent
        ]

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)
    |> fun s -> s.WithSubscribe subscribe


