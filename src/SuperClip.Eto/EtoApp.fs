[<AutoOpen>]
module SuperClip.Eto.EtoApp

open FSharp.Control.Tasks.V2

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

type HomePanel = SuperClip.Gui.Presenter.HomePanel.Presenter

type EtoApp (loggingArgs : LoggingArgs, args : AppArgs, platform : Eto.Platform) =
    inherit App (loggingArgs, args)
    let eto = new EtoPack<IApp, HomePanel>(platform, base.AsApp, HomePanel.Create)
    do (
        eto.Application.Name <- "SuperClip"
        eto.View.Title <- "SuperClip"
        eto.View.ClientSize <- Size (640, 800)
    )
    override this.SetupAsync' () = task {
        this.SetupSecureStorage ()
        eto.Setup ()
    }
    member __.Eto = eto
    static member Run (?platform : Eto.Platform) =
        let loggingArgs = LoggingArgs.CreateBoth ("super-clip-.log")
        let args = AppArgs.Create () |> AppArgs.SetScope "SuperClip"
        let platform = platform |> Option.defaultWith (fun () -> Eto.Platform.Detect)
        let app = new EtoApp (loggingArgs, args, platform)
        app.Eto.Run ()
        0