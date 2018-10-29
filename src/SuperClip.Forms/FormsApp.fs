[<AutoOpen>]
module SuperClip.Forms.FormsApp

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Forms
module FormsViewTypes = Dap.Forms.View.Types

open SuperClip.Core
open SuperClip.App
module ViewTypes = SuperClip.Forms.View.Types
module ViewLogic = SuperClip.Forms.View.Logic

type AppKinds with
    static member FormsView = "FormsView"

type AppKeys with
    static member FormsView = ""

type FormsApp (onAppStarted : FormsApp -> Unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
    inherit App ((fun app -> onAppStarted (app :?> FormsApp)), ?consoleLogLevel = consoleLogLevel, ?logFile = logFile)
    let formsArgs = ViewLogic.newArgs ()
    let mutable formsView : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> option = None
    do (
        base.SetupGuiContext' ()
    )
    override this.SetupAsync' () = task {
        let! formsView' = this.Env |> Env.addServiceAsync (Dap.Forms.View.Logic.spec this.AsSessionPack formsArgs) AppKinds.FormsView AppKeys.FormsView
        formsView <- Some formsView'
        do! formsView'.StartAsync ()
    }
    member __.FormsArgs = formsArgs
    member __.FormsView = formsView |> Option.get
    member __.View = formsView |> Option.get
    static member Create (onAppStarted, ?consoleLogLevel : LogLevel, ?logFile : string) =
        let consoleLogLevel = defaultArg consoleLogLevel LogLevelWarning
        new FormsApp (onAppStarted, consoleLogLevel = consoleLogLevel, ?logFile = logFile)

let createApplication () =
    FormsApp.Create ignore
    |> fun app -> app.FormsArgs.Application