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

let private doSetItem req ((item, callback) : Item * Callback<Result<SetItem.Res, SetItem.Err>>) : ActorOperate =
    fun runner (model, cmd) ->
        match item.Source with
        | Local ->
            reply runner callback <| ack req ^<| Error SetItem.InvalidSource
        | Cloud peer ->
            match Map.tryFind peer.Channel.Key model.Devices with
            | None ->
                reply runner callback <| ack req ^<| Error SetItem.InvalidChannel
            | Some (channel, device) ->
                if peer.Device.Guid <> device.Guid then
                    reply runner callback <| ack req ^<| Error SetItem.InvalidSource
                else
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

let private doRemoveDeviceFromChannel runner (channel : ChannelService) (device : Device) =
    channel.Actor.OnEvent.RemoveWatcher runner
    channel.Post <| ChannelTypes.DoRemoveDevice device None

let private onStatusChanged (status : LinkStatus) : ActorOperate =
    fun runner (model, cmd) ->
        match status with
        | LinkStatus.Closed ->
            model.Devices
            |> Map.iter (fun _k (channel, device) ->
                doRemoveDeviceFromChannel runner channel device
            )
            (runner, model, cmd)
            |=|> updateModel (fun m ->
                {m with
                    Devices = Map.empty
                }
            )
        | _ ->
            (model, cmd)

let private removeDeviceFromChannel runner (model : Model) (channelKey : string) =
    Map.tryFind channelKey model.Devices
    |> Option.map (fun (channel, device) ->
        doRemoveDeviceFromChannel runner channel device
        let devices =
            model.Devices
            |> Map.remove channelKey
        (devices, Some (channel, device))
    )|> Option.defaultValue (model.Devices, None)

let private addDevice ((channel, device) : ChannelService * Device) : ActorOperate =
    fun runner (model, cmd) ->
        let devices, _ = removeDeviceFromChannel runner model channel.Ident.Key
        channel.Post <| ChannelTypes.DoAddDevice device None
        channel.Actor.OnEvent.AddWatcher runner "CloudHub" (fun evt ->
            runner.Deliver <| ChannelEvt (channel, evt)
        )
        let devices =
            devices
            |> Map.add channel.Ident.Key (channel, device)
        (runner, model, cmd)
        |=|> updateModel (fun m -> {m with Devices = devices})

let private removeDevice ((channelKey, device) : ChannelKey * Device) : ActorOperate =
    fun runner (model, cmd) ->
        let devices, removed = removeDeviceFromChannel runner model channelKey
        match removed with
        | None ->
            logError runner "removeChannel" "Not_Found" (channelKey, device)
            (model, cmd)
        | Some (channel, removedDevice) ->
            if removedDevice.Guid <> device.Guid then
                logError runner "removeChannel" "Not_Matched" (channelKey, removedDevice, device)
                doRemoveDeviceFromChannel runner channel device
            (runner, model, cmd)
            |=|> updateModel (fun m -> {m with Devices = devices})

let private handleInternalEvt (evt : InternalEvt) : ActorOperate =
    match evt with
    | OnStatusChanged a -> onStatusChanged a
    | AddDevice (a, b) -> addDevice (a, b)
    | RemoveDevice (a, b) -> removeDevice (a, b)

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
        }, Cmd.none)

let spec pack (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn pack, args, HubReq, castEvt, init, update)