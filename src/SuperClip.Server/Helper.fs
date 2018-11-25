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
    static member CreateArgs (logFile, ?scope : string, ?consoleMinLevel : LogLevel) =
        let scope = defaultArg scope Scope
        let loggingArgs = LoggingArgs.CreateBoth (logFile, ?consoleMinLevel = consoleMinLevel)
        let args =
            AppArgs.Create ()
            |> fun a -> a.WithScope scope
            |> fun a -> a.WithFarangoDb (DbArgs.Create DbUri)
        (loggingArgs, args)
    static member Start (logFile, ?scope : string, ?consoleMinLevel : LogLevel) =
        let (l, a) = App.CreateArgs(logFile, ?scope = scope, ?consoleMinLevel = consoleMinLevel)
        let app = new App (l, a)
        app.Start ()
        app.AsApp