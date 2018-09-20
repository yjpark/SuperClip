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
    static member CreateAsync (logFile, consoleLogLevel, onAppStarted : IApp -> unit) = task {
        setSecret ()
        let loggingArgs = LoggingArgs.FormsCreate (logFile, consoleLogLevel)
        let app = new App (loggingArgs, Scope)
        do! app.SetupAsync AppArgs.Default
        let app = app.AsApp
        do! app.FormsView.StartAsync ()
        onAppStarted app
        return app
    }
    static member Create onAppStarted =
        App.CreateAsync ("super-clip-.log", LogLevelWarning, onAppStarted)
        |> runTask

let createApplication () =
    App.Create ignore
    |> fun app -> app.Args.FormsView.Application