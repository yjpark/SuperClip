[<AutoOpen>]
module SuperClip.Forms.App

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open SuperClip.Core

module FormsViewTypes = Dap.Forms.View.Types
module ViewTypes = SuperClip.Forms.View.Types
module TickerTypes = Dap.Platform.Ticker.Types
module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types
module Proxy = Dap.Remote.WebSocketProxy.Proxy
module CloudTypes = SuperClip.Core.Cloud.Types
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module SecureStorage = Dap.Forms.Provider.SecureStorage
module Context = Dap.Platform.Context
module SessionTypes = SuperClip.Forms.Session.Types

type IAppPackArgs =
    inherit ISessionPackArgs
    abstract FormsView : FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg> with get
    abstract AsSessionPackArgs : ISessionPackArgs with get

type IAppPack =
    inherit IPack
    inherit ISessionPack
    abstract Args : IAppPackArgs with get
    abstract FormsView : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> with get
    abstract AsSessionPack : ISessionPack with get

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type AppArgs = {
    Ticker : (* IServicesPack *) TickerTypes.Args
    PrimaryClipboard : (* ICorePack *) PrimaryTypes.Args
    LocalHistory : (* ICorePack *) HistoryTypes.Args
    History : (* ICorePack *) HistoryTypes.Args
    CloudStub : (* ICloudStubPack *) Proxy.Args<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>
    PacketClient : (* ICloudStubPack *) PacketClient.Args
    CredentialSecureStorage : (* IClientPack *) SecureStorage.Args<Credential>
    Preferences : (* IClientPack *) Context.Args<PrefContext>
    Session : (* ISessionPack *) NoArgs
    FormsView : (* IAppPack *) FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg>
} with
    static member Create ticker primaryClipboard localHistory history cloudStub packetClient credentialSecureStorage preferences session formsView
            : AppArgs =
        {
            Ticker = ticker
            PrimaryClipboard = primaryClipboard
            LocalHistory = localHistory
            History = history
            CloudStub = cloudStub
            PacketClient = packetClient
            CredentialSecureStorage = credentialSecureStorage
            Preferences = preferences
            Session = session
            FormsView = formsView
        }
    static member Default () =
        AppArgs.Create
            (TickerTypes.Args.Default ())
            (PrimaryTypes.Args.Default ())
            (HistoryTypes.Args.Default ())
            (HistoryTypes.Args.Default ())
            (Proxy.args CloudTypes.StubSpec (getCloudServerUri ()) (Some 5.000000<second>) true)
            (PacketClient.args true 1048576)
            (SecureStorage.args Credential.JsonEncoder Credential.JsonDecoder)
            spawnPrefContext
            NoArgs
            (SuperClip.Forms.View.Logic.newArgs ())
    static member JsonEncoder : JsonEncoder<AppArgs> =
        fun (this : AppArgs) ->
            E.object [
                "ticker", TickerTypes.Args.JsonEncoder this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder this.History
            ]
    static member JsonDecoder : JsonDecoder<AppArgs> =
        D.decode AppArgs.Create
        |> D.optional "ticker" TickerTypes.Args.JsonDecoder (TickerTypes.Args.Default ())
        |> D.optional "primary_clipboard" PrimaryTypes.Args.JsonDecoder (PrimaryTypes.Args.Default ())
        |> D.optional "local_history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
        |> D.optional "history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
        |> D.hardcoded (Proxy.args CloudTypes.StubSpec (getCloudServerUri ()) (Some 5.000000<second>) true)
        |> D.hardcoded (PacketClient.args true 1048576)
        |> D.hardcoded (SecureStorage.args Credential.JsonEncoder Credential.JsonDecoder)
        |> D.hardcoded spawnPrefContext
        |> D.hardcoded NoArgs
        |> D.hardcoded (SuperClip.Forms.View.Logic.newArgs ())
    static member JsonSpec =
        FieldSpec.Create<AppArgs>
            AppArgs.JsonEncoder AppArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = AppArgs.JsonEncoder this
    interface IObj
    member this.WithTicker ((* IServicesPack *) ticker : TickerTypes.Args) = {this with Ticker = ticker}
    member this.WithPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) = {this with PrimaryClipboard = primaryClipboard}
    member this.WithLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) = {this with LocalHistory = localHistory}
    member this.WithHistory ((* ICorePack *) history : HistoryTypes.Args) = {this with History = history}
    interface IServicesPackArgs with
        member this.Ticker (* IServicesPack *) : TickerTypes.Args = this.Ticker
    member this.AsServicesPackArgs = this :> IServicesPackArgs
    interface ICorePackArgs with
        member this.PrimaryClipboard (* ICorePack *) : PrimaryTypes.Args = this.PrimaryClipboard
        member this.LocalHistory (* ICorePack *) : HistoryTypes.Args = this.LocalHistory
        member this.History (* ICorePack *) : HistoryTypes.Args = this.History
        member this.AsServicesPackArgs = this.AsServicesPackArgs
    member this.AsCorePackArgs = this :> ICorePackArgs
    interface ICloudStubPackArgs with
        member this.CloudStub (* ICloudStubPack *) : Proxy.Args<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt> = this.CloudStub
        member this.PacketClient (* ICloudStubPack *) : PacketClient.Args = this.PacketClient
    member this.AsCloudStubPackArgs = this :> ICloudStubPackArgs
    interface IClientPackArgs with
        member this.CredentialSecureStorage (* IClientPack *) : SecureStorage.Args<Credential> = this.CredentialSecureStorage
        member this.Preferences (* IClientPack *) : Context.Args<PrefContext> = this.Preferences
        member this.AsCorePackArgs = this.AsCorePackArgs
        member this.AsCloudStubPackArgs = this.AsCloudStubPackArgs
    member this.AsClientPackArgs = this :> IClientPackArgs
    interface ISessionPackArgs with
        member this.Session (* ISessionPack *) : NoArgs = this.Session
        member this.AsClientPackArgs = this.AsClientPackArgs
    member this.AsSessionPackArgs = this :> ISessionPackArgs
    interface IAppPackArgs with
        member this.FormsView (* IAppPack *) : FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg> = this.FormsView
        member this.AsSessionPackArgs = this.AsSessionPackArgs
    member this.AsAppPackArgs = this :> IAppPackArgs

(*
 * Generated: <ValueBuilder>
 *)
type AppArgsBuilder () =
    inherit ObjBuilder<AppArgs> ()
    override __.Zero () = AppArgs.Default ()
    [<CustomOperation("ticker")>]
    member __.Ticker (target : AppArgs, (* IServicesPack *) ticker : TickerTypes.Args) =
        target.WithTicker ticker
    [<CustomOperation("primary_clipboard")>]
    member __.PrimaryClipboard (target : AppArgs, (* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        target.WithPrimaryClipboard primaryClipboard
    [<CustomOperation("local_history")>]
    member __.LocalHistory (target : AppArgs, (* ICorePack *) localHistory : HistoryTypes.Args) =
        target.WithLocalHistory localHistory
    [<CustomOperation("history")>]
    member __.History (target : AppArgs, (* ICorePack *) history : HistoryTypes.Args) =
        target.WithHistory history

let appArgs = AppArgsBuilder ()

type IApp =
    inherit IPack
    inherit IAppPack
    abstract Args : AppArgs with get
    abstract AsAppPack : IAppPack with get

type App (loggingArgs : LoggingArgs, scope : Scope) =
    let env = Env.live MailboxPlatform (loggingArgs.CreateLogging ()) scope
    let mutable args : AppArgs option = None
    let mutable setupError : exn option = None
    let mutable (* IServicesPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let mutable (* ICloudStubPack *) cloudStub : Proxy.Proxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt> option = None
    let mutable (* IClientPack *) credentialSecureStorage : SecureStorage.Service<Credential> option = None
    let mutable (* IClientPack *) preferences : Context.Agent<PrefContext> option = None
    let mutable (* ISessionPack *) session : SessionTypes.Agent option = None
    let mutable (* IAppPack *) formsView : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> option = None
    let setupAsync (this : App) : Task<unit> = task {
        let args' = args |> Option.get
        try
            let! (* IServicesPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args'.Ticker) "Ticker" ""
            ticker <- Some ticker'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsServicesPack args'.PrimaryClipboard) "Clipboard" "Primary"
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args'.LocalHistory) "History" "Local"
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args'.History) "History"
            let! (* ICloudStubPack *) cloudStub' = env |> Env.addServiceAsync (Dap.Remote.Proxy.Logic.spec args'.CloudStub) "CloudStub" ""
            cloudStub <- Some cloudStub'
            do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudStubPack *) args'.PacketClient) "PacketClient"
            let! (* IClientPack *) credentialSecureStorage' = env |> Env.addServiceAsync (Dap.Local.Storage.Base.Logic.spec args'.CredentialSecureStorage) "SecureStorage" "credential"
            credentialSecureStorage <- Some credentialSecureStorage'
            let! (* IClientPack *) preferences' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args'.Preferences) "preferences" ""
            preferences <- Some preferences'
            let! (* ISessionPack *) session' = env |> Env.addServiceAsync (SuperClip.Forms.Session.Logic.spec this.AsClientPack args'.Session) "Session" ""
            session <- Some session'
            let! (* IAppPack *) formsView' = env |> Env.addServiceAsync (Dap.Forms.View.Logic.spec this.AsSessionPack args'.FormsView) "FormsView" ""
            formsView <- Some formsView'
            do! this.SetupAsync' ()
            logInfo env "App.setupAsync" "Setup_Succeed" (E.encodeJson 4 args')
        with e ->
            setupError <- Some e
            logException env "App.setupAsync" "Setup_Failed" (E.encodeJson 4 args') e
    }
    member this.Setup (callback : IApp -> unit) (getArgs : unit -> AppArgs) : IApp =
        if args.IsSome then
            failWith "Already_Setup" <| E.encodeJson 4 args.Value
        else
            let args' = getArgs ()
            args <- Some args'
            env.RunTask0 raiseOnFailed (fun _ -> task {
                do! setupAsync this
                match setupError with
                | None -> callback this.AsApp
                | Some e -> raise e
            })
        this.AsApp
    member this.SetupArgs (callback : IApp -> unit) (args' : AppArgs) : IApp =
        fun () -> args'
        |> this.Setup callback
    member this.SetupJson (callback : IApp -> unit) (args' : Json) : IApp =
        fun () ->
            try
                castJson AppArgs.JsonDecoder args'
            with e ->
                logException env "App.Setup" "Decode_Failed" args e
                raise e
        |> this.Setup callback
    member this.SetupText (callback : IApp -> unit) (args' : string) : IApp =
        parseJson args'
        |> this.SetupJson callback
    member __.SetupError : exn option = setupError
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : AppArgs = args |> Option.get
    interface ILogger with
        member __.Log m = env.Log m
    interface IPack with
        member __.LoggingArgs : LoggingArgs = loggingArgs
        member __.Env : IEnv = env
    interface IServicesPack with
        member this.Args = this.Args.AsServicesPackArgs
        member __.Ticker (* IServicesPack *) : TickerTypes.Agent = ticker |> Option.get
    member this.AsServicesPack = this :> IServicesPack
    interface ICorePack with
        member this.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
        member this.AsServicesPack = this.AsServicesPack
    member this.AsCorePack = this :> ICorePack
    interface ICloudStubPack with
        member this.Args = this.Args.AsCloudStubPackArgs
        member __.CloudStub (* ICloudStubPack *) : Proxy.Proxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt> = cloudStub |> Option.get
        member __.GetPacketClientAsync (key : Key) (* ICloudStubPack *) : Task<PacketClient.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "PacketClient" key
            return (agent :?> PacketClient.Agent, isNew)
        }
    member this.AsCloudStubPack = this :> ICloudStubPack
    interface IClientPack with
        member this.Args = this.Args.AsClientPackArgs
        member __.CredentialSecureStorage (* IClientPack *) : SecureStorage.Service<Credential> = credentialSecureStorage |> Option.get
        member __.Preferences (* IClientPack *) : Context.Agent<PrefContext> = preferences |> Option.get
        member this.AsCorePack = this.AsCorePack
        member this.AsCloudStubPack = this.AsCloudStubPack
    member this.AsClientPack = this :> IClientPack
    interface ISessionPack with
        member this.Args = this.Args.AsSessionPackArgs
        member __.Session (* ISessionPack *) : SessionTypes.Agent = session |> Option.get
        member this.AsClientPack = this.AsClientPack
    member this.AsSessionPack = this :> ISessionPack
    interface IAppPack with
        member this.Args = this.Args.AsAppPackArgs
        member __.FormsView (* IAppPack *) : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> = formsView |> Option.get
        member this.AsSessionPack = this.AsSessionPack
    member this.AsAppPack = this :> IAppPack
    interface IApp with
        member this.Args : AppArgs = this.Args
        member this.AsAppPack = this.AsAppPack
    member this.AsApp = this :> IApp