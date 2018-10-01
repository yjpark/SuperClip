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

type FormsApp (loggingArgs : LoggingArgs, args : AppArgs) =
    inherit App (loggingArgs, args)
    static member Create (logFile, consoleLogLevel, onAppStarted : IApp -> Unit) =
        setSecret ()
        let loggingArgs = LoggingArgs.FormsCreate (logFile, consoleLogLevel)
        let args =
            AppArgs.Default ()
            |> AppArgs.SetScope Scope
            |> AppArgs.SetSetup onAppStarted
        new FormsApp (loggingArgs, args)
    static member Create (onAppStarted : IApp -> Unit) =
        FormsApp.Create ("super-clip-.log", LogLevelWarning, onAppStarted)
    override this.SetupAsync' () = task {
        do this.AsApp.FormsView.StartAsync ()
    }

let createApplication () =
    FormsApp.Create ignore
    |> fun app -> app.Args.FormsView.Application