module SuperClip.App.Session.Tasks

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App.Session.Types

module ChannelTypes = SuperClip.Core.Channel.Types
module ChannelService = SuperClip.Core.Channel.Service

let private isNotSelf (runner : Agent) (device : Device) =
    match runner.Actor.State.Auth with
    | Some auth -> auth.Device.Guid <> device.Guid
    | None -> true

let doSetChannelAsync (peers : Peers) : GetTask<Agent, unit> =
    fun runner -> task {
        let! channel = runner |> setupChannelServiceAsync peers.Channel
        runner.Deliver <| InternalEvt ^<| SetChannel (peers, channel)
        let devices =
            peers.Devices
            |> List.filter ^<| isNotSelf runner
        channel.Post <| ChannelTypes.DoSetDevices devices None
    }

let doAddToChannelAsync (peer : Peer) : GetTask<Agent, unit> =
    fun runner -> task {
        if isNotSelf runner peer.Device then
            match runner.Actor.State.Channel with
            | Some channel ->
                if channel.Ident.Key = peer.Channel.Key then
                    let! ok = channel.PostAsync <| ChannelTypes.DoAddDevice peer.Device
                    if ok then
                        runner.Deliver <| Evt ^<| OnDevicesChanged channel.Actor.State.Devices
                else
                    logError runner "doAddToChannelAsync" "Wrong_Channel" (channel.Ident.Key, peer)
            | None ->
                logError runner "doAddToChannelAsync" "No_Channel" peer
                ()
    }

let doRemoveFromChannelAsync (peer : Peer) : GetTask<Agent, unit> =
    fun runner -> task {
        if isNotSelf runner peer.Device then
            match runner.Actor.State.Channel with
            | Some channel ->
                if channel.Ident.Key = peer.Channel.Key then
                    let! ok = channel.PostAsync <| ChannelTypes.DoRemoveDevice peer.Device
                    if ok then
                        runner.Deliver <| Evt ^<| OnDevicesChanged channel.Actor.State.Devices
                else
                    logError runner "doRemoveFromChannelAsync" "Wrong_Channel" (channel.Ident.Key, peer)
            | None ->
                logError runner "doRemoveFromChannelAsync" "No_Channel" peer
                ()
    }
