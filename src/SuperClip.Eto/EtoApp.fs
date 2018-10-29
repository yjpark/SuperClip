[<AutoOpen>]
module SuperClip.Eto.EtoApp

open FSharp.Control.Tasks.V2

open Eto
open Eto.Forms

open Dap.Prelude
open Dap.Context
open Dap.Platform

open SuperClip.Core
open SuperClip.App
open SuperClip.Eto.Form

type EtoApp (platform : Platform, onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
    inherit App ((fun app -> onAppStarted (app :?> EtoApp)), ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)
    let application : Application = new Application (platform)
    let mutable mainForm : MainForm option = None
    do (
        base.SetupGuiContext' ()
        application.Run ()
    )
    override this.SetupAsync' () = task {
        let app = this.AsApp
        app.RunGuiFunc (fun _ ->
            let mainForm' = new MainForm (app)
            application.MainForm <- mainForm'
            mainForm <- Some mainForm'
            mainForm'.Show ()
        )
    }
    member __.Application = application
    member __.MainForm = mainForm |> Option.get
    static member Create (?platform : Platform, ?onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
        let platform = platform |> Option.defaultWith (fun () -> Platform.Detect)
        let onAppStarted = defaultArg onAppStarted ignore
        new EtoApp (platform, onAppStarted, ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)