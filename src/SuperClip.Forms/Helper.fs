[<AutoOpen>]
module SuperClip.Forms.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Forms

open SuperClip.Core
open SuperClip.Forms

[<Literal>]
let Scope = "SuperClip"

type IAppPack with
    member this.View = this.FormsView

let private setSecret () =
    Dap.Forms.Provider.SecureStorage.setSecret <| Des.encrypt "teiChe0xuo4maepezaihee8geigooTha" "mohtohJahmeechoch3sei3pheejaeGhu"

type App with
    static member Create (logFile, consoleLogLevel, onAppStarted) =
        setSecret ()
        let loggingArgs = LoggingArgs.FormsCreate (logFile, consoleLogLevel)
        new App (loggingArgs, Scope)
        |> fun app -> app.Setup (fun a -> a.View.Run a onAppStarted) AppArgs.Default
    static member Create onAppStarted =
        App.Create ("super-clip-.log", LogLevelWarning, onAppStarted)

let createApplication () =
    App.Create ("super-clip-.log", LogLevelWarning, ignore)
    |> fun app -> app.Args.FormsView.Application