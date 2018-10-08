module SuperClip.Forms.Session.Tasks

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Forms.Session.Types

module ChannelTypes = SuperClip.Core.Channel.Types
module ChannelService = SuperClip.Core.Channel.Service

let doSetChannelAsync (peers : Peers) : GetTask<Agent, unit> =
    fun runner -> task {
        let! channel = runner |> setupChannelServiceAsync peers.Channel
        runner.Deliver <| InternalEvt ^<| SetChannel (peers, channel)
        channel.Post <| ChannelTypes.DoSetDevices peers.Devices None
    }