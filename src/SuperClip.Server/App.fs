[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Server.App

open Farango.Connection
open Dap.Prelude
open Dap.Platform
open Dap.Archive

open Farango.Cdc

module CloudHubAgent = SuperClip.Server.CloudHub.Agent
module RegistryService = Dap.Platform.Registry.Service

[<Literal>]
let UserEnv = "UserEnv"

let init' consoleLogLevel logFile =
    let timestamp = Profile.perMinute.CalcVolumeKey <| getNow' ()
    let ident = timestamp.Replace(":", "_")
    let logging =
        setupSerilog
            [
                addConsoleSink <| Some consoleLogLevel
                addDailyFileSink <| sprintf "log/%s/%s" ident logFile
                //addSeqSink "http://localhost:5341"
            ]
    let env =
        Env.live MailboxPlatform logging "SuperClip"

    let dbUri = "http://superclip_dev:Ex2Kuth1ZeiN0Ishie9pahng9xea5xu5@localhost:8529/superclip_dev"
    let db =
        try
            connect dbUri |> Async.RunSynchronously
        with e -> Error e.Message
    let app =
        match db with
        | Ok connection ->
            {
                Env = env
                Db = connection
                Ident = ident
            }
        | Error err ->
            logError env "DB" "Connect_Failed" (dbUri, err)
            failwith <| sprintf "Connected To DB Failed: %s -> %s" dbUri err

    Async.AwaitTask <| CloudHubAgent.doRegisterAsync app
    |> Async.RunSynchronously
    app

let init logFile = init' LogLevelError logFile