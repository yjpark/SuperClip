[<AutoOpen>]
module SuperClip.Forms.App

open Dap.Context.Helper
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
module Cloud = SuperClip.Core.Cloud
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

type AppKinds () =
    static member Ticker (* IServicesPack *) = "Ticker"
    static member PrimaryClipboard (* ICorePack *) = "Clipboard"
    static member LocalHistory (* ICorePack *) = "History"
    static member History (* ICorePack *) = "History"
    static member CloudStub (* ICloudStubPack *) = "CloudStub"
    static member PacketClient (* ICloudStubPack *) = "PacketClient"
    static member CredentialSecureStorage (* IClientPack *) = "SecureStorage"
    static member Preferences (* IClientPack *) = "Preferences"
    static member Session (* ISessionPack *) = "Session"
    static member FormsView (* IAppPack *) = "FormsView"

type AppKeys () =
    static member Ticker (* IServicesPack *) = ""
    static member PrimaryClipboard (* ICorePack *) = "Primary"
    static member LocalHistory (* ICorePack *) = "Local"
    static member CloudStub (* ICloudStubPack *) = ""
    static member CredentialSecureStorage (* IClientPack *) = "Credential"
    static member Preferences (* IClientPack *) = ""
    static member Session (* ISessionPack *) = ""
    static member FormsView (* IAppPack *) = ""

type IApp =
    inherit IPack
    inherit IAppPack
    abstract Args : AppArgs with get
    abstract AsAppPack : IAppPack with get

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
and AppArgs = {
    Scope : (* AppArgs *) Scope
    Setup : (* AppArgs *) IApp -> unit
    Ticker : (* IServicesPack *) TickerTypes.Args
    PrimaryClipboard : (* ICorePack *) PrimaryTypes.Args
    LocalHistory : (* ICorePack *) HistoryTypes.Args
    History : (* ICorePack *) HistoryTypes.Args
    CloudStub : (* ICloudStubPack *) Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>
    PacketClient : (* ICloudStubPack *) PacketClient.Args
    CredentialSecureStorage : (* IClientPack *) SecureStorage.Args<Credential>
    Preferences : (* IClientPack *) Context.Args<PrefContext>
    Session : (* ISessionPack *) NoArgs
    FormsView : (* IAppPack *) FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg>
} with
    static member Create scope setup ticker primaryClipboard localHistory history cloudStub packetClient credentialSecureStorage preferences session formsView
            : AppArgs =
        {
            Scope = (* AppArgs *) scope
            Setup = (* AppArgs *) setup
            Ticker = (* IServicesPack *) ticker
            PrimaryClipboard = (* ICorePack *) primaryClipboard
            LocalHistory = (* ICorePack *) localHistory
            History = (* ICorePack *) history
            CloudStub = (* ICloudStubPack *) cloudStub
            PacketClient = (* ICloudStubPack *) packetClient
            CredentialSecureStorage = (* IClientPack *) credentialSecureStorage
            Preferences = (* IClientPack *) preferences
            Session = (* ISessionPack *) session
            FormsView = (* IAppPack *) formsView
        }
    static member Default () =
        AppArgs.Create
            NoScope (* AppArgs *) (* scope *)
            ignore (* AppArgs *) (* setup *)
            (TickerTypes.Args.Default ()) (* IServicesPack *) (* ticker *)
            (PrimaryTypes.Args.Default ()) (* ICorePack *) (* primaryClipboard *)
            (HistoryTypes.Args.Default ()) (* ICorePack *) (* localHistory *)
            (HistoryTypes.Args.Default ()) (* ICorePack *) (* history *)
            (Proxy.args Cloud.StubSpec (getCloudServerUri ()) (Some 5.000000<second>) true) (* ICloudStubPack *) (* cloudStub *)
            (PacketClient.args true 1048576) (* ICloudStubPack *) (* packetClient *)
            (SecureStorage.args Credential.JsonEncoder Credential.JsonDecoder) (* IClientPack *) (* credentialSecureStorage *)
            spawnPrefContext (* IClientPack *) (* preferences *)
            NoArgs (* ISessionPack *) (* session *)
            (SuperClip.Forms.View.Logic.newArgs ()) (* IAppPack *) (* formsView *)
    static member SetScope ((* AppArgs *) scope : Scope) (this : AppArgs) =
        {this with Scope = scope}
    static member SetSetup ((* AppArgs *) setup : IApp -> unit) (this : AppArgs) =
        {this with Setup = setup}
    static member SetTicker ((* IServicesPack *) ticker : TickerTypes.Args) (this : AppArgs) =
        {this with Ticker = ticker}
    static member SetPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) (this : AppArgs) =
        {this with PrimaryClipboard = primaryClipboard}
    static member SetLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) (this : AppArgs) =
        {this with LocalHistory = localHistory}
    static member SetHistory ((* ICorePack *) history : HistoryTypes.Args) (this : AppArgs) =
        {this with History = history}
    static member SetCloudStub ((* ICloudStubPack *) cloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) (this : AppArgs) =
        {this with CloudStub = cloudStub}
    static member SetPacketClient ((* ICloudStubPack *) packetClient : PacketClient.Args) (this : AppArgs) =
        {this with PacketClient = packetClient}
    static member SetCredentialSecureStorage ((* IClientPack *) credentialSecureStorage : SecureStorage.Args<Credential>) (this : AppArgs) =
        {this with CredentialSecureStorage = credentialSecureStorage}
    static member SetPreferences ((* IClientPack *) preferences : Context.Args<PrefContext>) (this : AppArgs) =
        {this with Preferences = preferences}
    static member SetSession ((* ISessionPack *) session : NoArgs) (this : AppArgs) =
        {this with Session = session}
    static member SetFormsView ((* IAppPack *) formsView : FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg>) (this : AppArgs) =
        {this with FormsView = formsView}
    static member UpdateScope ((* AppArgs *) update : Scope -> Scope) (this : AppArgs) =
        this |> AppArgs.SetScope (update this.Scope)
    static member UpdateTicker ((* IServicesPack *) update : TickerTypes.Args -> TickerTypes.Args) (this : AppArgs) =
        this |> AppArgs.SetTicker (update this.Ticker)
    static member UpdatePrimaryClipboard ((* ICorePack *) update : PrimaryTypes.Args -> PrimaryTypes.Args) (this : AppArgs) =
        this |> AppArgs.SetPrimaryClipboard (update this.PrimaryClipboard)
    static member UpdateLocalHistory ((* ICorePack *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : AppArgs) =
        this |> AppArgs.SetLocalHistory (update this.LocalHistory)
    static member UpdateHistory ((* ICorePack *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : AppArgs) =
        this |> AppArgs.SetHistory (update this.History)
    static member UpdateCloudStub ((* ICloudStubPack *) update : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt> -> Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) (this : AppArgs) =
        this |> AppArgs.SetCloudStub (update this.CloudStub)
    static member UpdatePacketClient ((* ICloudStubPack *) update : PacketClient.Args -> PacketClient.Args) (this : AppArgs) =
        this |> AppArgs.SetPacketClient (update this.PacketClient)
    static member UpdateCredentialSecureStorage ((* IClientPack *) update : SecureStorage.Args<Credential> -> SecureStorage.Args<Credential>) (this : AppArgs) =
        this |> AppArgs.SetCredentialSecureStorage (update this.CredentialSecureStorage)
    static member UpdatePreferences ((* IClientPack *) update : Context.Args<PrefContext> -> Context.Args<PrefContext>) (this : AppArgs) =
        this |> AppArgs.SetPreferences (update this.Preferences)
    static member UpdateSession ((* ISessionPack *) update : NoArgs -> NoArgs) (this : AppArgs) =
        this |> AppArgs.SetSession (update this.Session)
    static member UpdateFormsView ((* IAppPack *) update : FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg> -> FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg>) (this : AppArgs) =
        this |> AppArgs.SetFormsView (update this.FormsView)
    static member JsonEncoder : JsonEncoder<AppArgs> =
        fun (this : AppArgs) ->
            E.object [
                "scope", Scope.JsonEncoder (* AppArgs *) this.Scope
                "ticker", TickerTypes.Args.JsonEncoder (* IServicesPack *) this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder (* ICorePack *) this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.History
            ]
    static member JsonDecoder : JsonDecoder<AppArgs> =
        D.object (fun get ->
            {
                Scope = get.Optional.Field (* AppArgs *) "scope" Scope.JsonDecoder
                    |> Option.defaultValue NoScope
                Setup = (* (* AppArgs *)  *) ignore
                Ticker = get.Optional.Field (* IServicesPack *) "ticker" TickerTypes.Args.JsonDecoder
                    |> Option.defaultValue (TickerTypes.Args.Default ())
                PrimaryClipboard = get.Optional.Field (* ICorePack *) "primary_clipboard" PrimaryTypes.Args.JsonDecoder
                    |> Option.defaultValue (PrimaryTypes.Args.Default ())
                LocalHistory = get.Optional.Field (* ICorePack *) "local_history" HistoryTypes.Args.JsonDecoder
                    |> Option.defaultValue (HistoryTypes.Args.Default ())
                History = get.Optional.Field (* ICorePack *) "history" HistoryTypes.Args.JsonDecoder
                    |> Option.defaultValue (HistoryTypes.Args.Default ())
                CloudStub = (* (* ICloudStubPack *)  *) (Proxy.args Cloud.StubSpec (getCloudServerUri ()) (Some 5.000000<second>) true)
                PacketClient = (* (* ICloudStubPack *)  *) (PacketClient.args true 1048576)
                CredentialSecureStorage = (* (* IClientPack *)  *) (SecureStorage.args Credential.JsonEncoder Credential.JsonDecoder)
                Preferences = (* (* IClientPack *)  *) spawnPrefContext
                Session = (* (* ISessionPack *)  *) NoArgs
                FormsView = (* (* IAppPack *)  *) (SuperClip.Forms.View.Logic.newArgs ())
            }
        )
    static member JsonSpec =
        FieldSpec.Create<AppArgs> (AppArgs.JsonEncoder, AppArgs.JsonDecoder)
    interface IJson with
        member this.ToJson () = AppArgs.JsonEncoder this
    interface IObj
    member this.WithScope ((* AppArgs *) scope : Scope) =
        this |> AppArgs.SetScope scope
    member this.WithSetup ((* AppArgs *) setup : IApp -> unit) =
        this |> AppArgs.SetSetup setup
    member this.WithTicker ((* IServicesPack *) ticker : TickerTypes.Args) =
        this |> AppArgs.SetTicker ticker
    member this.WithPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        this |> AppArgs.SetPrimaryClipboard primaryClipboard
    member this.WithLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) =
        this |> AppArgs.SetLocalHistory localHistory
    member this.WithHistory ((* ICorePack *) history : HistoryTypes.Args) =
        this |> AppArgs.SetHistory history
    member this.WithCloudStub ((* ICloudStubPack *) cloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) =
        this |> AppArgs.SetCloudStub cloudStub
    member this.WithPacketClient ((* ICloudStubPack *) packetClient : PacketClient.Args) =
        this |> AppArgs.SetPacketClient packetClient
    member this.WithCredentialSecureStorage ((* IClientPack *) credentialSecureStorage : SecureStorage.Args<Credential>) =
        this |> AppArgs.SetCredentialSecureStorage credentialSecureStorage
    member this.WithPreferences ((* IClientPack *) preferences : Context.Args<PrefContext>) =
        this |> AppArgs.SetPreferences preferences
    member this.WithSession ((* ISessionPack *) session : NoArgs) =
        this |> AppArgs.SetSession session
    member this.WithFormsView ((* IAppPack *) formsView : FormsViewTypes.Args<ISessionPack, ViewTypes.Model, ViewTypes.Msg>) =
        this |> AppArgs.SetFormsView formsView
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
        member this.CloudStub (* ICloudStubPack *) : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt> = this.CloudStub
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
    [<CustomOperation("scope")>]
    member __.Scope (target : AppArgs, (* AppArgs *) scope : Scope) =
        target.WithScope scope
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

let app_args = AppArgsBuilder ()

type App (logging : ILogging, args : AppArgs) as this =
    let env = Env.live MailboxPlatform logging args.Scope
    let mutable setupError : exn option = None
    let mutable (* IServicesPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let mutable (* ICloudStubPack *) cloudStub : Proxy.Proxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt> option = None
    let mutable (* IClientPack *) credentialSecureStorage : SecureStorage.Service<Credential> option = None
    let mutable (* IClientPack *) preferences : Context.Agent<PrefContext> option = None
    let mutable (* ISessionPack *) session : SessionTypes.Agent option = None
    let mutable (* IAppPack *) formsView : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> option = None
    let setupAsync (_runner : IRunner) : Task<unit> = task {
        try
            let! (* IServicesPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args.Ticker) AppKinds.Ticker AppKeys.Ticker
            ticker <- Some ticker'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsServicesPack args.PrimaryClipboard) AppKinds.PrimaryClipboard AppKeys.PrimaryClipboard
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args.LocalHistory) AppKinds.LocalHistory AppKeys.LocalHistory
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args.History) AppKinds.History
            let! (* ICloudStubPack *) cloudStub' = env |> Env.addServiceAsync (Dap.Remote.Proxy.Logic.Logic.spec args.CloudStub) AppKinds.CloudStub AppKeys.CloudStub
            cloudStub <- Some cloudStub'
            do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudStubPack *) args.PacketClient) AppKinds.PacketClient
            let! (* IClientPack *) credentialSecureStorage' = env |> Env.addServiceAsync (Dap.Local.Storage.Base.Logic.spec args.CredentialSecureStorage) AppKinds.CredentialSecureStorage AppKeys.CredentialSecureStorage
            credentialSecureStorage <- Some credentialSecureStorage'
            let! (* IClientPack *) preferences' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.Preferences) AppKinds.Preferences AppKeys.Preferences
            preferences <- Some preferences'
            let! (* ISessionPack *) session' = env |> Env.addServiceAsync (SuperClip.Forms.Session.Logic.spec this.AsClientPack args.Session) AppKinds.Session AppKeys.Session
            session <- Some session'
            let! (* IAppPack *) formsView' = env |> Env.addServiceAsync (Dap.Forms.View.Logic.spec this.AsSessionPack args.FormsView) AppKinds.FormsView AppKeys.FormsView
            formsView <- Some formsView'
            do! this.SetupAsync' ()
            logInfo env "App.setupAsync" "Setup_Succeed" (E.encodeJson 4 args)
            args.Setup this.AsApp
        with e ->
            setupError <- Some e
            logException env "App.setupAsync" "Setup_Failed" (E.encodeJson 4 args) e
            raise e
    }
    do (
        env.RunTask0 raiseOnFailed setupAsync
    )
    new (loggingArgs : LoggingArgs, a : AppArgs) = new App (loggingArgs.CreateLogging (), a)
    new (a : AppArgs) = new App (getLogging (), a)
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : AppArgs = args
    member __.Env : IEnv = env
    member __.SetupError : exn option = setupError
    interface ILogger with
        member __.Log m = env.Log m
    interface IPack with
        member __.Env : IEnv = env
    interface IServicesPack with
        member __.Args = this.Args.AsServicesPackArgs
        member __.Ticker (* IServicesPack *) : TickerTypes.Agent = ticker |> Option.get
    member __.AsServicesPack = this :> IServicesPack
    interface ICorePack with
        member __.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
        member __.AsServicesPack = this.AsServicesPack
    member __.AsCorePack = this :> ICorePack
    interface ICloudStubPack with
        member __.Args = this.Args.AsCloudStubPackArgs
        member __.CloudStub (* ICloudStubPack *) : Proxy.Proxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt> = cloudStub |> Option.get
        member __.GetPacketClientAsync (key : Key) (* ICloudStubPack *) : Task<PacketClient.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "PacketClient" key
            return (agent :?> PacketClient.Agent, isNew)
        }
    member __.AsCloudStubPack = this :> ICloudStubPack
    interface IClientPack with
        member __.Args = this.Args.AsClientPackArgs
        member __.CredentialSecureStorage (* IClientPack *) : SecureStorage.Service<Credential> = credentialSecureStorage |> Option.get
        member __.Preferences (* IClientPack *) : Context.Agent<PrefContext> = preferences |> Option.get
        member __.AsCorePack = this.AsCorePack
        member __.AsCloudStubPack = this.AsCloudStubPack
    member __.AsClientPack = this :> IClientPack
    interface ISessionPack with
        member __.Args = this.Args.AsSessionPackArgs
        member __.Session (* ISessionPack *) : SessionTypes.Agent = session |> Option.get
        member __.AsClientPack = this.AsClientPack
    member __.AsSessionPack = this :> ISessionPack
    interface IAppPack with
        member __.Args = this.Args.AsAppPackArgs
        member __.FormsView (* IAppPack *) : FormsViewTypes.View<ISessionPack, ViewTypes.Model, ViewTypes.Msg> = formsView |> Option.get
        member __.AsSessionPack = this.AsSessionPack
    member __.AsAppPack = this :> IAppPack
    interface IApp with
        member __.Args : AppArgs = this.Args
        member __.AsAppPack = this.AsAppPack
    member __.AsApp = this :> IApp