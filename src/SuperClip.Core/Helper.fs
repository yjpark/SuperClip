[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Archive

module PrimaryAgent = SuperClip.Core.Primary.Agent
module TickerService = Dap.Platform.Ticker.Service

let doRegisterAsync (env : IEnv) = task {
    let args = TickerService.Args.New (10.0)
    let! _ = env |> TickerService.addAsync noKey args
    let args = PrimaryAgent.Args.New ()
    do! env |> PrimaryAgent.registerAsync args
}

let doSpawnPrimaryAsync (env : IEnv) = task {
    let! (agent, isNew) = env.HandleAsync <| DoGetAgent' PrimaryAgent.Kind noKey
    return agent :?> PrimaryAgent.Agent
}

let initLocal logFile =
    let timestamp = Profile.perMinute.CalcVolumeKey <| getNow' ()
    let ident = timestamp.Replace(":", "_")
    let logging =
        setupSerilog
            [
                addConsoleSink <| Some LogLevelError
                addDailyFileSink <| sprintf "log/%s/%s" ident logFile
                //addSeqSink "http://localhost:5341"
            ]
    let env = Env.live MailboxPlatform logging "SuperClip"

    Async.AwaitTask <| doRegisterAsync env
    |> Async.RunSynchronously

    let primary =
        Async.AwaitTask <| doSpawnPrimaryAsync env
        |> Async.RunSynchronously

    (env, primary)