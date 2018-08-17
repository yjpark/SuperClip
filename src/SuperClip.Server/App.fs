[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Server.App

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Archive
open Dap.Local.App
open Dap.Local.Farango
open Dap.Local.Farango.App

module CloudHubAgent = SuperClip.Server.CloudHub.Agent
module RegistryService = Dap.Platform.Registry.Service

[<Literal>]
let Scope = "SuperClipServer"

type Args = WithDb.Args

type Model = WithDb.Model

let init = WithDb.init

let setupAsync (app : Model) = task {
    do! app |> CloudHubAgent.doRegisterAsync
}

let create' consoleLogLevel logFile =
    let dbUri = "http://superclip_dev:Ex2Kuth1ZeiN0Ishie9pahng9xea5xu5@localhost:8529/superclip_dev"
    (Args.Create
        (Simple.Args.Default Scope consoleLogLevel logFile)
        (DbArgs.Create dbUri)
        setupAsync
    )|> init

let create logFile =
    create' LogLevelWarning logFile
