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

let doSetupPrimaryAsync (env : IEnv) = task {
    let args = TickerService.Args.New (10.0)
    let! _ = env |> TickerService.addAsync noKey args
    let args = PrimaryService.Args.New ()
    let! primary = env |> PrimaryService.addAsync noKey args
    return primary
}

let doSetupAsync (env : IEnv) = task {
    let! _ = env |> doSetupPrimaryAsync
    let args = HistoryAgent.Args.New ()
    do! env |> HistoryAgent.registerAsync args
    let! (history, _isNew) = env.HandleAsync <| DoGetAgent' HistoryAgent.Kind noKey
    return history :?> HistoryAgent.Agent
}