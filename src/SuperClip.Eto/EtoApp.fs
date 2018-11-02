[<AutoOpen>]
module SuperClip.Eto.EtoApp

open FSharp.Control.Tasks.V2

open System.Threading
open Eto
open Eto.Forms
open Eto.Drawing

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Eto

open SuperClip.Core
open SuperClip.App
open SuperClip.Eto.Form

type EmptyForm () =
    inherit Form ()
    do (
        base.Title <- "Empty"
        base.ClientSize <- Size (800, 600)
    )

type EtoApp (platform : Platform, onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
    inherit App ((fun app -> onAppStarted (app :?> EtoApp)), ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)
    let mutable application : Application option = None
    let mutable mainForm : MainForm option = None
    do (
        application <- Some <| createEtoApplication (platform)
        mainForm <- Some <| new MainForm (base.Env)
        application.Value.Run (mainForm.Value)
    )
    override this.SetupAsync' () = task {
        application.Value.Invoke (fun () ->
            let app = this.AsApp
            app.SetupGuiContext' ()
            mainForm.Value.Attach app
        )
    }
    member __.Application = application |> Option.get
    member __.MainForm = mainForm |> Option.get
    static member Create (?platform : Platform, ?onAppStarted : EtoApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
        let platform = platform |> Option.defaultWith (fun () -> Platform.Detect)
        let onAppStarted = defaultArg onAppStarted ignore
        new EtoApp (platform, onAppStarted, ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)