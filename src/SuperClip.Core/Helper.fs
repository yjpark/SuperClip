[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Archive

module TickerService = Dap.Platform.Ticker.Service

module PrimaryService = SuperClip.Core.Primary.Service
module HistoryAgent = SuperClip.Core.History.Agent
module ChannelService = SuperClip.Core.Channel.Service

let doSetupPrimaryAsync (env : IEnv) = task {
    let args = TickerService.Args.New (10.0)
    let! _ = env |> TickerService.addAsync NoKey args
    let args = PrimaryService.Args.New ()
    let! primary = env |> PrimaryService.addAsync NoKey args
    return primary
}

let doSetupAsync (env : IEnv) = task {
    let! _ = env |> doSetupPrimaryAsync
    let args = HistoryAgent.Args.New ()
    do! env |> HistoryAgent.registerAsync args
    let! (history, _isNew) = env.HandleAsync <| DoGetAgent HistoryAgent.Kind NoKey
    return history :?> HistoryAgent.Agent
}

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