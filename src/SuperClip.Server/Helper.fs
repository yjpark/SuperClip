[<AutoOpen>]
module SuperClip.Server.Helper

open FSharp.Control.Tasks.V2
open Farango.Types

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
module ChannelService = SuperClip.Core.Channel.Service

let private initChannelServiceAsync (channel : Channel) : GetTask<IAgent, ChannelService.Service> =
    fun runner -> task {
        let! service = runner.Env |> ChannelService.addAsync channel.Key channel
        return service
    }

let setupChannelServiceAsync (channel : Channel) : GetTask<IAgent, ChannelService.Service> =
    fun runner -> task {
        let service = runner.Env |> ChannelService.tryFind channel.Key
        match service with
        | Some service ->
            return service
        | None ->
            let! service = runner |> initChannelServiceAsync channel
            return service
    }