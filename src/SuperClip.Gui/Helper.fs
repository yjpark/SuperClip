[<AutoOpen>]
module SuperClip.Gui.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.App

open SuperClip.App
type HomePanel = SuperClip.Gui.Presenter.HomePanel.Presenter

type App with
    static member RunGui (logFile, ?scope : string, ?consoleMinLevel : LogLevel) : int =
        App.Create (logFile, ?scope = scope, ?consoleMinLevel = consoleMinLevel)
        :> IApp
        |> runGuiApp HomePanel.Create