[<AutoOpen>]
module SuperClip.Server.App

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
    static member Create scope setup farangoDb ticker packetConn cloudHub cloudHubGateway
            : AppArgs =
        {
            Scope = (* AppArgs *) scope
            Setup = (* AppArgs *) setup
            FarangoDb = (* IDbPack *) farangoDb
            Ticker = (* ITickingPack *) ticker
            PacketConn = (* ICloudHubPack *) packetConn
            CloudHub = (* ICloudHubPack *) cloudHub
            CloudHubGateway = (* ICloudHubPack *) cloudHubGateway
        }
    static member Default () =
        AppArgs.Create
            NoScope (* AppArgs *) (* scope *)
            ignore (* AppArgs *) (* setup *)
            (FarangoDb.Args.Default ()) (* IDbPack *) (* farangoDb *)
            (TickerTypes.Args.Default ()) (* ITickingPack *) (* ticker *)
            (PacketConn.args true 1048576 (decodeJsonString Duration.JsonDecoder """0:00:00:05""")) (* ICloudHubPack *) (* packetConn *)
            NoArgs (* ICloudHubPack *) (* cloudHub *)
            (Gateway.args CloudHubTypes.HubSpec true) (* ICloudHubPack *) (* cloudHubGateway *)
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
    static member UpdateScope ((* AppArgs *) update : Scope -> Scope) (this : AppArgs) =
        this |> AppArgs.SetScope (update this.Scope)
    static member UpdateFarangoDb ((* IDbPack *) update : FarangoDb.Args -> FarangoDb.Args) (this : AppArgs) =
        this |> AppArgs.SetFarangoDb (update this.FarangoDb)
    static member UpdateTicker ((* ITickingPack *) update : TickerTypes.Args -> TickerTypes.Args) (this : AppArgs) =
        this |> AppArgs.SetTicker (update this.Ticker)
    static member UpdatePacketConn ((* ICloudHubPack *) update : PacketConn.Args -> PacketConn.Args) (this : AppArgs) =
        this |> AppArgs.SetPacketConn (update this.PacketConn)
    static member UpdateCloudHub ((* ICloudHubPack *) update : NoArgs -> NoArgs) (this : AppArgs) =
        this |> AppArgs.SetCloudHub (update this.CloudHub)
    static member UpdateCloudHubGateway ((* ICloudHubPack *) update : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt> -> Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>) (this : AppArgs) =
        this |> AppArgs.SetCloudHubGateway (update this.CloudHubGateway)
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
    [<CustomOperation("farango_db")>]
    member __.FarangoDb (target : AppArgs, (* IDbPack *) farangoDb : FarangoDb.Args) =
        target.WithFarangoDb farangoDb
    [<CustomOperation("ticker")>]
    member __.Ticker (target : AppArgs, (* ITickingPack *) ticker : TickerTypes.Args) =
        target.WithTicker ticker

let app_args = AppArgsBuilder ()

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