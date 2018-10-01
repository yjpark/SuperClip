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
    static member Create (logFile, consoleLogLevel) =
        let loggingArgs = LoggingArgs.LocalCreate (logFile, consoleLogLevel)
        let args =
            AppArgs.Default ()
            |> fun a -> a.WithFarangoDb (DbArgs.Create DbUri)
        new App (loggingArgs, args)
    static member Create logFile =
        App.Create (logFile, LogLevelWarning)