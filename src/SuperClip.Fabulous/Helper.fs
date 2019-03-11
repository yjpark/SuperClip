[<AutoOpen>]
module SuperClip.Fabulous.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Fabulous

open SuperClip.App
module ViewTypes = SuperClip.Fabulous.View.Types
module ViewLogic = SuperClip.Fabulous.View.Logic

type App with
    static member RunFabulous (logFile, ?scope : string, ?consoleMinLevel : LogLevel) : int =
        setFabulousParam <| ViewLogic.newArgs ()
        App.Create (logFile, ?scope = scope, ?consoleMinLevel = consoleMinLevel)
        :> IApp
        |> runFabulousApp<IApp, ViewTypes.Model, ViewTypes.Msg>

let runForUwp (logFile) =
    App.RunFabulous (logFile) |> ignore
    let fabulousParam = getFabulousParam ()
    fabulousParam.Application
