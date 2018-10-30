[<AutoOpen>]
module SuperClip.Eto.EtoApp

open FSharp.Control.Tasks.V2

open System.Threading
open Eto
open Eto.Forms

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
open SuperClip.Eto.Form

type EtoApp (platform : Platform, onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
    inherit App ((fun app -> onAppStarted (app :?> EtoApp)), ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)
    let mutable application : Application option = None
    let mutable mainForm : MainForm option = None
    do (
        application <- Some <| new Application (platform)
        //Note: Gtk thread been init when application created, so have to setup gui context after
        base.SetupGuiContext' ()
        application.Value.Run ()
    )
    override this.SetupAsync' () = task {
        let app = this.AsApp
        app.RunGuiFunc (fun _ ->
            let mainForm' = new MainForm (app)
            application.Value.MainForm <- mainForm'
            mainForm <- Some mainForm'
            mainForm'.Show ()
        )
    }
    member __.Application = application |> Option.get
    member __.MainForm = mainForm |> Option.get
    static member Create (?platform : Platform, ?onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
        let platform = platform |> Option.defaultWith (fun () -> Platform.Detect)
        let onAppStarted = defaultArg onAppStarted ignore
        new EtoApp (platform, onAppStarted, ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)