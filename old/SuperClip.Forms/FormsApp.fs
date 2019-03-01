[<AutoOpen>]
module SuperClip.Forms.FormsApp

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Forms
module FormsViewTypes = Dap.Forms.Fabulous.View.Types

open SuperClip.Core
open SuperClip.App
module ViewTypes = SuperClip.Forms.View.Types
module ViewLogic = SuperClip.Forms.View.Logic

type AppKinds with
    static member FormsView = "FormsView"

type AppKeys with
    static member FormsView = ""

type FormsApp (loggingArgs : LoggingArgs, args : AppArgs, onAppStarted : FormsApp -> Unit) =
    inherit App (loggingArgs, args)
    let formsArgs = ViewLogic.newArgs ()
    let mutable formsView : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> option = None
    do (
        base.SetupGuiContext' ()
    )
    override this.SetupAsync' () = task {
        logWip this "SetupAsync'" ()
        let! formsView' = this.Env |> Env.addServiceAsync (Dap.Forms.Fabulous.View.Logic.spec this.AsSessionPack formsArgs) AppKinds.FormsView AppKeys.FormsView
        formsView <- Some formsView'
        do! formsView'.StartAsync ()
        onAppStarted this
    }
    member __.FormsArgs = formsArgs
    member __.FormsView = formsView |> Option.get
    member __.View = formsView |> Option.get
    static member Run (onAppStarted : FormsApp -> Unit) =
        let loggingArgs = LoggingArgs.CreateBoth ("super-clip-.log")
        let args = AppArgs.Create () |> AppArgs.SetScope "SuperClip"
        let app = new FormsApp (loggingArgs, args, onAppStarted)
        Feature.startApp app
        logWip app "Run" ()
        app

let createApplication () =
    FormsApp.Run ignore
    |> fun app -> app.FormsArgs.Application