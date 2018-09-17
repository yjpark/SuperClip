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
    member this.Application = this.FormsView.Actor.Args.Application

let private setSecret () =
    Dap.Forms.Provider.SecureStorage.setSecret <| Des.encrypt "teiChe0xuo4maepezaihee8geigooTha" "mohtohJahmeechoch3sei3pheejaeGhu"

let createApp' logFile consoleLogLevel onAppStarted : IApp =
    setSecret ()
    let logging = createFormsLogging logFile consoleLogLevel
    let app = new App (logging, Scope)
    app.Setup onAppStarted AppArgs.Default
let createApp onAppStarted =
    createApp' "super-clip-.log" LogLevelWarning onAppStarted

let createApplication () =
    createApp' "super-clip-.log" LogLevelWarning ignore
    |> fun app -> app.Application