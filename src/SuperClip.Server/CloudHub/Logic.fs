[<RequireQualifiedAccess>]
module SuperClip.Server.CloudHub.Logic

open Elmish
open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Remote.Server.Auth

open SuperClip.Core
open SuperClip.Core.Cloud
module CloudTypes = SuperClip.Core.Cloud.Types
module ChannelTypes = SuperClip.Core.Channel.Types

open SuperClip.Server.CloudHub.Types
open SuperClip.Server.CloudHub.Tasks

module ChannelTypes = SuperClip.Core.Channel.Types
type ChannelService = SuperClip.Core.Channel.Service.Service

type ActorOperate = ActorOperate<Agent, Args, Model, Msg, Req, Evt>

let private doSetItem req ((item, callback) : Item * Callback<Result<SetItem.Res, SetItem.Error>>) : ActorOperate =
    fun runner (model, cmd) ->
        match item.Source with
        | Local ->
            reply runner callback <| ack req ^<| Error SetItem.InvalidSource
        | Cloud peer ->
            match Map.tryFind peer.Device.Guid model.Devices with
            | None ->
                reply runner callback <| ack req ^<| Error SetItem.AuthRequired
            | Some device ->
                match Map.tryFind peer.Channel.Key model.Channels with
                | None ->
                    reply runner callback <| ack req ^<| Error SetItem.InvalidChannel
                | Some channel ->
                    channel.Post <| ChannelTypes.DoSet item None
                    reply runner callback <| ack req ^<| Ok JsonNil
        (model, cmd)

let private handleReq (req : Req) : ActorOperate =
    fun runner (model, cmd) ->
        match req with
        | CloudTypes.ServerReq.DoJoin (r, callback) ->
            replyAsync runner req callback nakOnFailed <| doJoinAsync r
            (model, cmd)
        | CloudTypes.ServerReq.DoAuth (r, callback) ->
            replyAsync runner req callback nakOnFailed <| doAuthAsync r
            (model, cmd)
        | CloudTypes.ServerReq.DoLeave (r, callback) ->
            replyAsync runner req callback nakOnFailed <| doLeaveAsync r
            (model, cmd)
        | CloudTypes.ServerReq.DoSetItem (r, callback) ->
            (runner, model, cmd)
            |=|> doSetItem req (r, callback)

let private onDisconnected : ActorOperate =
    fun runner (model, cmd) ->
        model.Channels
        |> Map.iter (fun _k channel ->
            channel.Actor.OnEvent.RemoveWatcher runner
        )
        (runner, model, cmd)
        |=|> updateModel (fun m ->
            {m with
                Devices = Map.empty
                Channels = Map.empty
            }
        )

let private addChannel (channel : ChannelService) : ActorOperate =
    fun runner (model, cmd) ->
        match Map.tryFind channel.Ident.Key model.Channels with
        | None ->
            channel.Actor.OnEvent.AddWatcher runner "CloudHub" (fun evt ->
                runner.Deliver <| ChannelEvt (channel, evt)
            )
            let channels =
                model.Channels
                |> Map.add channel.Ident.Key channel
            (runner, model, cmd)
            |=|> updateModel (fun m -> {m with Channels = channels})
        | Some channel ->
            (model, cmd)

let private removeChannel (channel : ChannelService) : ActorOperate =
    fun runner (model, cmd) ->
        match Map.tryFind channel.Ident.Key model.Channels with
        | None ->
            (model, cmd)
        | Some channel ->
            channel.Actor.OnEvent.RemoveWatcher runner
            let channels =
                model.Channels
                |> Map.remove channel.Ident.Key
            (runner, model, cmd)
            |=|> updateModel (fun m -> {m with Channels = channels})

let private addDevice (device : Device) : ActorOperate =
    fun runner (model, cmd) ->
        let devices =
            model.Devices
            |> Map.add device.Guid device
        (runner, model, cmd)
        |=|> updateModel (fun m -> {m with Devices = devices})

let private removeDevice (device : Device) : ActorOperate =
    fun runner (model, cmd) ->
        let devices =
            model.Devices
            |> Map.remove device.Guid
        (runner, model, cmd)
        |=|> updateModel (fun m -> {m with Devices = devices})

let private handleInternalEvt (evt : InternalEvt) : ActorOperate =
    match evt with
    | OnDisconnected -> onDisconnected
    | AddChannel channel -> addChannel channel
    | RemoveChannel channel -> removeChannel channel
    | AddDevice device -> addDevice device
    | RemoveDevice device -> removeDevice device

let private handleChannelEvt (channel : ChannelService) (evt : ChannelTypes.Evt) : ActorOperate =
    match evt with
    | ChannelTypes.OnChanged item ->
        addSubCmd HubEvt <| CloudTypes.OnItemChanged item
    | ChannelTypes.OnDeviceAdded device ->
        addSubCmd HubEvt <| CloudTypes.OnPeerJoin ^<| Peer.Create channel.Channel device
    | ChannelTypes.OnDeviceRemoved device ->
        addSubCmd HubEvt <| CloudTypes.OnPeerLeft ^<| Peer.Create channel.Channel device

let private update : ActorUpdate<Agent, Args, Model, Msg, Req, Evt> =
    fun runner msg model ->
        match msg with
        | HubReq req ->
            handleReq req
        | HubEvt _evt ->
            noOperation
        | InternalEvt evt ->
            handleInternalEvt evt
        | ChannelEvt (a, b) ->
            handleChannelEvt a b
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    fun _runner args ->
        ({
            Devices = Map.empty
            Channels = Map.empty
        }, Cmd.none)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, HubReq, castEvt, init, update)