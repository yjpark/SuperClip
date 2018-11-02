[<RequireQualifiedAccess>]
module SuperClip.App.BaseApp

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local
open SuperClip.Core

module TickerTypes = Dap.Platform.Ticker.Types
module Context = Dap.Platform.Context
module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types
module Proxy = Dap.Remote.WebSocketProxy.Proxy
module Cloud = SuperClip.Core.Cloud
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module SessionTypes = SuperClip.App.Session.Types

(*
 * Generated: <App>
 *)
type App (logging : ILogging, args : AppArgs) as this =
    let env = Env.live MailboxPlatform logging args.Scope
    let mutable setupError : exn option = None
    let mutable (* ITickingPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ILocalPack *) localClipboard : Context.Agent<ILocalClipboard> option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let mutable (* ICloudStubPack *) cloudStub : Proxy.Proxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt> option = None
    let mutable (* IAppPack *) preferences : Context.Agent<IPreferences> option = None
    let mutable (* IAppPack *) secureStorage : Context.Agent<ISecureStorage> option = None
    let mutable (* IClientPack *) userPref : Context.Agent<IUserPref> option = None
    let mutable (* ISessionPack *) session : SessionTypes.Agent option = None
    let mutable (* IGuiPack *) appGui : Context.Agent<IAppGui> option = None
    let setupAsync (_runner : IRunner) : Task<unit> = task {
        try
            let! (* ITickingPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args.Ticker) AppKinds.Ticker AppKeys.Ticker
            ticker <- Some ticker'
            let! (* ILocalPack *) localClipboard' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.LocalClipboard) AppKinds.LocalClipboard AppKeys.LocalClipboard
            localClipboard <- Some localClipboard'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsLocalPack args.PrimaryClipboard) AppKinds.PrimaryClipboard AppKeys.PrimaryClipboard
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args.LocalHistory) AppKinds.LocalHistory AppKeys.LocalHistory
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args.History) AppKinds.History
            let! (* ICloudStubPack *) cloudStub' = env |> Env.addServiceAsync (Dap.Remote.Proxy.Logic.Logic.spec args.CloudStub) AppKinds.CloudStub AppKeys.CloudStub
            cloudStub <- Some cloudStub'
            do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudStubPack *) this.AsTickingPack args.PacketClient) AppKinds.PacketClient
            let! (* IAppPack *) preferences' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.Preferences) AppKinds.Preferences AppKeys.Preferences
            preferences <- Some preferences'
            let! (* IAppPack *) secureStorage' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.SecureStorage) AppKinds.SecureStorage AppKeys.SecureStorage
            secureStorage <- Some secureStorage'
            let! (* IClientPack *) userPref' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.UserPref) AppKinds.UserPref AppKeys.UserPref
            userPref <- Some userPref'
            let! (* ISessionPack *) session' = env |> Env.addServiceAsync (SuperClip.App.Session.Logic.spec this.AsClientPack args.Session) AppKinds.Session AppKeys.Session
            session <- Some session'
            let! (* IGuiPack *) appGui' = env |> Env.addServiceAsync (Dap.Platform.Context.spec args.AppGui) AppKinds.AppGui AppKeys.AppGui
            appGui <- Some appGui'
            do! this.SetupAsync' ()
            logInfo env "App.setupAsync" "Setup_Succeed" (encodeJson 4 args)
            args.Setup this.AsApp
        with e ->
            setupError <- Some e
            logException env "App.setupAsync" "Setup_Failed" (encodeJson 4 args) e
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
    interface IRunner<IApp> with
        member __.Runner = this.AsApp
        member __.RunFunc func = runFunc' this func
        member __.AddTask onFailed getTask = addTask' this onFailed getTask
        member __.RunTask onFailed getTask = runTask' this onFailed getTask
    interface IRunner with
        member __.Clock = env.Clock
        member __.Console0 = env.Console0
        member __.RunFunc0 func = runFunc' this func
        member __.AddTask0 onFailed getTask = addTask' this onFailed getTask
        member __.RunTask0 onFailed getTask = runTask' this onFailed getTask
    interface ITaskManager with
        member __.StartTask task = env.StartTask task
        member __.ScheduleTask task = env.ScheduleTask task
        member __.PendingTasksCount = env.PendingTasksCount
        member __.StartPendingTasks () = env.StartPendingTasks ()
        member __.ClearPendingTasks () = env.ClearPendingTasks ()
        member __.RunningTasksCount = env.RunningTasksCount
        member __.CancelRunningTasks () = env.CancelRunningTasks ()
    interface IPack with
        member __.Env : IEnv = env
    interface ITickingPack with
        member __.Args = this.Args.AsTickingPackArgs
        member __.Ticker (* ITickingPack *) : TickerTypes.Agent = ticker |> Option.get
    member __.AsTickingPack = this :> ITickingPack
    interface ILocalPack with
        member __.Args = this.Args.AsLocalPackArgs
        member __.LocalClipboard (* ILocalPack *) : Context.Agent<ILocalClipboard> = localClipboard |> Option.get
        member __.AsTickingPack = this.AsTickingPack
    member __.AsLocalPack = this :> ILocalPack
    interface ICorePack with
        member __.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
        member __.AsLocalPack = this.AsLocalPack
    member __.AsCorePack = this :> ICorePack
    interface ICloudStubPack with
        member __.Args = this.Args.AsCloudStubPackArgs
        member __.CloudStub (* ICloudStubPack *) : Proxy.Proxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt> = cloudStub |> Option.get
        member __.GetPacketClientAsync (key : Key) (* ICloudStubPack *) : Task<PacketClient.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "PacketClient" key
            return (agent :?> PacketClient.Agent, isNew)
        }
        member __.AsTickingPack = this.AsTickingPack
    member __.AsCloudStubPack = this :> ICloudStubPack
    interface IAppPack with
        member __.Args = this.Args.AsAppPackArgs
        member __.Preferences (* IAppPack *) : Context.Agent<IPreferences> = preferences |> Option.get
        member __.SecureStorage (* IAppPack *) : Context.Agent<ISecureStorage> = secureStorage |> Option.get
    member __.AsAppPack = this :> IAppPack
    interface IClientPack with
        member __.Args = this.Args.AsClientPackArgs
        member __.UserPref (* IClientPack *) : Context.Agent<IUserPref> = userPref |> Option.get
        member __.AsCorePack = this.AsCorePack
        member __.AsCloudStubPack = this.AsCloudStubPack
        member __.AsAppPack = this.AsAppPack
    member __.AsClientPack = this :> IClientPack
    interface ISessionPack with
        member __.Args = this.Args.AsSessionPackArgs
        member __.Session (* ISessionPack *) : SessionTypes.Agent = session |> Option.get
        member __.AsClientPack = this.AsClientPack
    member __.AsSessionPack = this :> ISessionPack
    interface IGuiPack with
        member __.Args = this.Args.AsGuiPackArgs
        member __.AppGui (* IGuiPack *) : Context.Agent<IAppGui> = appGui |> Option.get
    member __.AsGuiPack = this :> IGuiPack
    interface IApp with
        member __.Args : AppArgs = this.Args
        member __.AsSessionPack = this.AsSessionPack
        member __.AsGuiPack = this.AsGuiPack
    member __.AsApp = this :> IApp