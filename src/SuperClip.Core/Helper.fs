[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Archive

module TickerTypes = Dap.Platform.Ticker.Types

module HistoryAgent = SuperClip.Core.History.Agent
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