[<AutoOpen>]
module SuperClip.App.App

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local

open SuperClip.Core
open SuperClip.App

[<Literal>]
let Scope = "SuperClip"

type LinkSessionStatus =
    | NoLink
    | NoAuth
    | NoChannel of Credential
    | Syncing of Credential
    | Pausing of Credential

type IApp with
    member this.LinkSessionStatus : LinkSessionStatus =
        let session = this.Session
        if this.CloudStub.Status = LinkStatus.Linked then
            match session.Actor.State.Auth with
            | Some auth ->
                match session.Actor.State.Channel with
                | Some channel ->
                    match session.Actor.State.Syncing with
                    | true ->
                        Syncing auth
                    | false ->
                        Pausing auth
                | None ->
                    NoChannel auth
            | None ->
                NoAuth
        else
            NoLink
    member this.Gui = this.AsGuiPack.AppGui.Context

type App (loggingArgs : LoggingArgs, args : AppArgs) =
    inherit SuperClip.App.BaseApp.App (loggingArgs, args.WithScope Scope)
    override this.SetupAsync' () = task {
        let app = this.AsApp
        Des.encrypt "teiChe0xuo4maepezaihee8geigooTha" "mohtohJahmeechoch3sei3pheejaeGhu"
        |> app.SecureStorageProps.Secret.SetValue
        UserPref.setup app.SecureStorage.Context app.UserPref
    }
    new (onAppStarted : IApp -> unit, ?consoleLogLevel : LogLevel, ?logFile : string) =
        let logFile = logFile |> Option.defaultValue "super-clip-.log"
    #if FEATURE_DAP_FORMS
        let loggingArgs = LoggingArgs.FormsCreate (?consoleLogLevel = consoleLogLevel, filename = logFile, rolling = RollingInterval.Daily)
    #else
        let loggingArgs = LoggingArgs.LocalCreate (?consoleLogLevel = consoleLogLevel, filename = logFile, rolling = RollingInterval.Daily)
    #endif
        let args =
            AppArgs.Default ()
            |> AppArgs.SetSetup onAppStarted
        new App (loggingArgs, args)
