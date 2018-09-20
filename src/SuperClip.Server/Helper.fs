[<AutoOpen>]
module SuperClip.Server.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Local
open Dap.Local.Farango

[<Literal>]
let Scope = "SuperClipServer"

let DbUri = "http://superclip_dev:Ex2Kuth1ZeiN0Ishie9pahng9xea5xu5@localhost:8529/superclip_dev"

type App with
    static member CreateAsync (logFile, consoleLogLevel) = task {
        let loggingArgs = LoggingArgs.LocalCreate (logFile, consoleLogLevel)
        let app = new App (loggingArgs, Scope)
        let args =
            AppArgs.Default ()
            |> fun a -> a.WithFarangoDb (DbArgs.Create DbUri)
        do! app.SetupAsync args
        return app.AsApp
    }
    static member Create (logFile, consoleLogLevel) =
        App.CreateAsync (logFile, consoleLogLevel)
        |> runTask
    static member Create logFile =
        App.Create (logFile, LogLevelWarning)