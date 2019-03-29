[<AutoOpen>]
module SuperClip.App.Helper

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
    | NoChannel
    | Joined of bool * bool

type IApp with
    member this.SetupSecureStorage () =
        Des.encrypt "teiChe0xuo4maepezaihee8geigooTha" "mohtohJahmeechoch3sei3pheejaeGhu"
        |> IEnvironment.Instance.SecureStorageProps.Secret.SetValue
        UserPref.setup this this.UserPref
    member this.LinkSessionStatus : LinkSessionStatus =
        let session = this.Session
        if this.CloudStub.Status = LinkStatus.Linked then
            match session.Actor.State.Auth with
            | Some auth ->
                match session.Actor.State.Channel with
                | Some channel ->
                    Joined (session.Actor.State.SyncingUp, session.Actor.State.SyncingDown)
                | None ->
                    NoChannel
            | None ->
                NoAuth
        else
            NoLink
    member this.Gui = this.AsGuiPack.AppGui.Context

type App with
    static member Create (logFile, ?scope : string, ?consoleMinLevel : LogLevel) =
        let scope = defaultArg scope Scope
        let loggingArgs = LoggingArgs.CreateBoth (logFile, ?consoleMinLevel = consoleMinLevel)
        let args =
            AppArgs.Create ()
            |> fun a -> a.WithScope scope
            |> fun a -> a.WithSetup (fun app -> app.SetupSecureStorage ())
        new App (loggingArgs, args)

let getSuperClipAppAssembly () =
    typeof<SuperClip.App.Session.Types.Model>.Assembly