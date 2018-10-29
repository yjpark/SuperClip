[<AutoOpen>]
module SuperClip.Server.App

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local.Farango
open SuperClip.Core

module FarangoDb = Dap.Local.Farango.Db
module TickerTypes = Dap.Platform.Ticker.Types
module PacketConn = Dap.Remote.WebSocketGateway.PacketConn
module CloudHubTypes = SuperClip.Server.CloudHub.Types
module Gateway = Dap.Remote.WebSocketGateway.Gateway

(*
 * Generated: <App>
 *)

type AppKinds () =
    static member FarangoDb (* IDbPack *) = "FarangoDb"
    static member Ticker (* ITickingPack *) = "Ticker"
    static member PacketConn (* ICloudHubPack *) = "PacketConn"
    static member CloudHub (* ICloudHubPack *) = "CloudHub"
    static member CloudHubGateway (* ICloudHubPack *) = "CloudHubGateway"

type AppKeys () =
    static member FarangoDb (* IDbPack *) = ""
    static member Ticker (* ITickingPack *) = ""

type IApp =
    inherit IPack
    inherit IDbPack
    inherit ICloudHubPack
    abstract Args : AppArgs with get
    abstract AsDbPack : IDbPack with get
    abstract AsCloudHubPack : ICloudHubPack with get

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
and AppArgs = {
    Scope : (* AppArgs *) Scope
    Setup : (* AppArgs *) IApp -> unit
    FarangoDb : (* IDbPack *) FarangoDb.Args
    Ticker : (* ITickingPack *) TickerTypes.Args
    PacketConn : (* ICloudHubPack *) PacketConn.Args
    CloudHub : (* ICloudHubPack *) NoArgs
    CloudHubGateway : (* ICloudHubPack *) Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>
} with
    static member Create
        (
            ?scope : (* AppArgs *) Scope,
            ?setup : (* AppArgs *) IApp -> unit,
            ?farangoDb : (* IDbPack *) FarangoDb.Args,
            ?ticker : (* ITickingPack *) TickerTypes.Args,
            ?packetConn : (* ICloudHubPack *) PacketConn.Args,
            ?cloudHub : (* ICloudHubPack *) NoArgs,
            ?cloudHubGateway : (* ICloudHubPack *) Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>
        ) : AppArgs =
        {
            Scope = (* AppArgs *) scope
                |> Option.defaultWith (fun () -> NoScope)
            Setup = (* AppArgs *) setup
                |> Option.defaultWith (fun () -> ignore)
            FarangoDb = (* IDbPack *) farangoDb
                |> Option.defaultWith (fun () -> (FarangoDb.Args.Default ()))
            Ticker = (* ITickingPack *) ticker
                |> Option.defaultWith (fun () -> (TickerTypes.Args.Default ()))
            PacketConn = (* ICloudHubPack *) packetConn
                |> Option.defaultWith (fun () -> (PacketConn.args true 1048576 (decodeJsonString Duration.JsonDecoder """0:00:00:05""")))
            CloudHub = (* ICloudHubPack *) cloudHub
                |> Option.defaultWith (fun () -> NoArgs)
            CloudHubGateway = (* ICloudHubPack *) cloudHubGateway
                |> Option.defaultWith (fun () -> (Gateway.args CloudHubTypes.HubSpec true))
        }
    static member Default () = AppArgs.Create ()
    static member SetScope ((* AppArgs *) scope : Scope) (this : AppArgs) =
        {this with Scope = scope}
    static member SetSetup ((* AppArgs *) setup : IApp -> unit) (this : AppArgs) =
        {this with Setup = setup}
    static member SetFarangoDb ((* IDbPack *) farangoDb : FarangoDb.Args) (this : AppArgs) =
        {this with FarangoDb = farangoDb}
    static member SetTicker ((* ITickingPack *) ticker : TickerTypes.Args) (this : AppArgs) =
        {this with Ticker = ticker}
    static member SetPacketConn ((* ICloudHubPack *) packetConn : PacketConn.Args) (this : AppArgs) =
        {this with PacketConn = packetConn}
    static member SetCloudHub ((* ICloudHubPack *) cloudHub : NoArgs) (this : AppArgs) =
        {this with CloudHub = cloudHub}
    static member SetCloudHubGateway ((* ICloudHubPack *) cloudHubGateway : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>) (this : AppArgs) =
        {this with CloudHubGateway = cloudHubGateway}
    static member JsonEncoder : JsonEncoder<AppArgs> =
        fun (this : AppArgs) ->
            E.object [
                "scope", Scope.JsonEncoder (* AppArgs *) this.Scope
                "farango_db", FarangoDb.Args.JsonEncoder (* IDbPack *) this.FarangoDb
                "ticker", TickerTypes.Args.JsonEncoder (* ITickingPack *) this.Ticker
            ]
    static member JsonDecoder : JsonDecoder<AppArgs> =
        D.object (fun get ->
            {
                Scope = get.Optional.Field (* AppArgs *) "scope" Scope.JsonDecoder
                    |> Option.defaultValue NoScope
                Setup = (* (* AppArgs *)  *) ignore
                FarangoDb = get.Optional.Field (* IDbPack *) "farango_db" FarangoDb.Args.JsonDecoder
                    |> Option.defaultValue (FarangoDb.Args.Default ())
                Ticker = get.Optional.Field (* ITickingPack *) "ticker" TickerTypes.Args.JsonDecoder
                    |> Option.defaultValue (TickerTypes.Args.Default ())
                PacketConn = (* (* ICloudHubPack *)  *) (PacketConn.args true 1048576 (decodeJsonString Duration.JsonDecoder """0:00:00:05"""))
                CloudHub = (* (* ICloudHubPack *)  *) NoArgs
                CloudHubGateway = (* (* ICloudHubPack *)  *) (Gateway.args CloudHubTypes.HubSpec true)
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
    member this.WithFarangoDb ((* IDbPack *) farangoDb : FarangoDb.Args) =
        this |> AppArgs.SetFarangoDb farangoDb
    member this.WithTicker ((* ITickingPack *) ticker : TickerTypes.Args) =
        this |> AppArgs.SetTicker ticker
    member this.WithPacketConn ((* ICloudHubPack *) packetConn : PacketConn.Args) =
        this |> AppArgs.SetPacketConn packetConn
    member this.WithCloudHub ((* ICloudHubPack *) cloudHub : NoArgs) =
        this |> AppArgs.SetCloudHub cloudHub
    member this.WithCloudHubGateway ((* ICloudHubPack *) cloudHubGateway : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>) =
        this |> AppArgs.SetCloudHubGateway cloudHubGateway
    interface IDbPackArgs with
        member this.FarangoDb (* IDbPack *) : FarangoDb.Args = this.FarangoDb
    member this.AsDbPackArgs = this :> IDbPackArgs
    interface ITickingPackArgs with
        member this.Ticker (* ITickingPack *) : TickerTypes.Args = this.Ticker
    member this.AsTickingPackArgs = this :> ITickingPackArgs
    interface ICloudHubPackArgs with
        member this.PacketConn (* ICloudHubPack *) : PacketConn.Args = this.PacketConn
        member this.CloudHub (* ICloudHubPack *) : NoArgs = this.CloudHub
        member this.CloudHubGateway (* ICloudHubPack *) : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt> = this.CloudHubGateway
        member this.AsTickingPackArgs = this.AsTickingPackArgs
    member this.AsCloudHubPackArgs = this :> ICloudHubPackArgs

(*
 * Generated: <ValueBuilder>
 *)
type AppArgsBuilder () =
    inherit ObjBuilder<AppArgs> ()
    override __.Zero () = AppArgs.Default ()
    [<CustomOperation("scope")>]
    member __.Scope (target : AppArgs, (* AppArgs *) scope : Scope) =
        target.WithScope scope
    [<CustomOperation("Setup")>]
    member __.Setup (target : AppArgs, (* AppArgs *) setup : IApp -> unit) =
        target.WithSetup setup
    [<CustomOperation("farango_db")>]
    member __.FarangoDb (target : AppArgs, (* IDbPack *) farangoDb : FarangoDb.Args) =
        target.WithFarangoDb farangoDb
    [<CustomOperation("ticker")>]
    member __.Ticker (target : AppArgs, (* ITickingPack *) ticker : TickerTypes.Args) =
        target.WithTicker ticker
    [<CustomOperation("packet_conn")>]
    member __.PacketConn (target : AppArgs, (* ICloudHubPack *) packetConn : PacketConn.Args) =
        target.WithPacketConn packetConn
    [<CustomOperation("cloud_hub")>]
    member __.CloudHub (target : AppArgs, (* ICloudHubPack *) cloudHub : NoArgs) =
        target.WithCloudHub cloudHub
    [<CustomOperation("cloud_hub_gateway")>]
    member __.CloudHubGateway (target : AppArgs, (* ICloudHubPack *) cloudHubGateway : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>) =
        target.WithCloudHubGateway cloudHubGateway

let app_args = new AppArgsBuilder ()

(*
 * Generated: <App>
 *)
type App (logging : ILogging, args : AppArgs) as this =
    let env = Env.live MailboxPlatform logging args.Scope
    let mutable setupError : exn option = None
    let mutable (* IDbPack *) farangoDb : FarangoDb.Agent option = None
    let mutable (* ITickingPack *) ticker : TickerTypes.Agent option = None
    let setupAsync (_runner : IRunner) : Task<unit> = task {
        try
            let! (* IDbPack *) farangoDb' = env |> Env.addServiceAsync (FarangoDb.spec args.FarangoDb) AppKinds.FarangoDb AppKeys.FarangoDb
            farangoDb <- Some farangoDb'
            let! (* ITickingPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args.Ticker) AppKinds.Ticker AppKeys.Ticker
            ticker <- Some ticker'
            do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudHubPack *) this.AsTickingPack args.PacketConn) AppKinds.PacketConn
            do! env |> Env.registerAsync (SuperClip.Server.CloudHub.Logic.spec (* ICloudHubPack *) this.AsDbPack args.CloudHub) AppKinds.CloudHub
            do! env |> Env.registerAsync (Dap.Remote.WebSocketGateway.Logic.spec (* ICloudHubPack *) args.CloudHubGateway) AppKinds.CloudHubGateway
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
    interface IDbPack with
        member __.Args = this.Args.AsDbPackArgs
        member __.FarangoDb (* IDbPack *) : FarangoDb.Agent = farangoDb |> Option.get
    member __.AsDbPack = this :> IDbPack
    interface ITickingPack with
        member __.Args = this.Args.AsTickingPackArgs
        member __.Ticker (* ITickingPack *) : TickerTypes.Agent = ticker |> Option.get
    member __.AsTickingPack = this :> ITickingPack
    interface ICloudHubPack with
        member __.Args = this.Args.AsCloudHubPackArgs
        member __.GetPacketConnAsync (key : Key) (* ICloudHubPack *) : Task<PacketConn.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "PacketConn" key
            return (agent :?> PacketConn.Agent, isNew)
        }
        member __.GetCloudHubAsync (key : Key) (* ICloudHubPack *) : Task<CloudHubTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "CloudHub" key
            return (agent :?> CloudHubTypes.Agent, isNew)
        }
        member __.GetCloudHubGatewayAsync (key : Key) (* ICloudHubPack *) : Task<Gateway.Gateway * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "CloudHubGateway" key
            return (agent :?> Gateway.Gateway, isNew)
        }
        member __.AsTickingPack = this.AsTickingPack
    member __.AsCloudHubPack = this :> ICloudHubPack
    interface IApp with
        member __.Args : AppArgs = this.Args
        member __.AsDbPack = this.AsDbPack
        member __.AsCloudHubPack = this.AsCloudHubPack
    member __.AsApp = this :> IApp