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
module PacketConn = Dap.Remote.WebSocketGateway.PacketConn
module CloudHubTypes = SuperClip.Server.CloudHub.Types
module Gateway = Dap.Remote.WebSocketGateway.Gateway

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type AppArgs = {
    FarangoDb : (* IDbPack *) FarangoDb.Args
    PacketConn : (* ICloudHubPack *) PacketConn.Args
    CloudHub : (* ICloudHubPack *) NoArgs
    CloudHubGateway : (* ICloudHubPack *) Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt>
} with
    static member Create farangoDb packetConn cloudHub cloudHubGateway
            : AppArgs =
        {
            FarangoDb = farangoDb
            PacketConn = packetConn
            CloudHub = cloudHub
            CloudHubGateway = cloudHubGateway
        }
    static member Default () =
        AppArgs.Create
            (FarangoDb.Args.Default ())
            (PacketConn.args true 1048576)
            NoArgs
            (Gateway.args CloudHubTypes.HubSpec true)
    static member JsonEncoder : JsonEncoder<AppArgs> =
        fun (this : AppArgs) ->
            E.object [
                "farango_db", FarangoDb.Args.JsonEncoder this.FarangoDb
            ]
    static member JsonDecoder : JsonDecoder<AppArgs> =
        D.decode AppArgs.Create
        |> D.optional "farango_db" FarangoDb.Args.JsonDecoder (FarangoDb.Args.Default ())
        |> D.hardcoded (PacketConn.args true 1048576)
        |> D.hardcoded NoArgs
        |> D.hardcoded (Gateway.args CloudHubTypes.HubSpec true)
    static member JsonSpec =
        FieldSpec.Create<AppArgs>
            AppArgs.JsonEncoder AppArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = AppArgs.JsonEncoder this
    interface IObj
    member this.WithFarangoDb ((* IDbPack *) farangoDb : FarangoDb.Args) = {this with FarangoDb = farangoDb}
    interface IDbPackArgs with
        member this.FarangoDb (* IDbPack *) : FarangoDb.Args = this.FarangoDb
    member this.AsDbPackArgs = this :> IDbPackArgs
    interface ICloudHubPackArgs with
        member this.PacketConn (* ICloudHubPack *) : PacketConn.Args = this.PacketConn
        member this.CloudHub (* ICloudHubPack *) : NoArgs = this.CloudHub
        member this.CloudHubGateway (* ICloudHubPack *) : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt> = this.CloudHubGateway
    member this.AsCloudHubPackArgs = this :> ICloudHubPackArgs

(*
 * Generated: <ValueBuilder>
 *)
type AppArgsBuilder () =
    inherit ObjBuilder<AppArgs> ()
    override __.Zero () = AppArgs.Default ()
    [<CustomOperation("farango_db")>]
    member __.FarangoDb (target : AppArgs, (* IDbPack *) farangoDb : FarangoDb.Args) =
        target.WithFarangoDb farangoDb

let appArgs = AppArgsBuilder ()

type IApp =
    inherit IPack
    inherit IDbPack
    inherit ICloudHubPack
    abstract Args : AppArgs with get
    abstract AsDbPack : IDbPack with get
    abstract AsCloudHubPack : ICloudHubPack with get

type App (logging : ILogging, scope : Scope) =
    let env = Env.live MailboxPlatform logging scope
    let mutable args : AppArgs option = None
    let mutable setupError : exn option = None
    let mutable (* IDbPack *) farangoDb : FarangoDb.Agent option = None
    let setupAsync (this : App) : Task<unit> = task {
        let args' = args |> Option.get
        try
            let! (* IDbPack *) farangoDb' = env |> Env.addServiceAsync (FarangoDb.spec args'.FarangoDb) "FarangoDb" ""
            farangoDb <- Some farangoDb'
            do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudHubPack *) args'.PacketConn) "PacketConn"
            do! env |> Env.registerAsync (SuperClip.Server.CloudHub.Logic.spec (* ICloudHubPack *) this.AsDbPack args'.CloudHub) "CloudHub"
            do! env |> Env.registerAsync (Dap.Remote.WebSocketGateway.Logic.spec (* ICloudHubPack *) args'.CloudHubGateway) "CloudHubGateway"
            do! this.SetupAsync' ()
            logInfo env "App.setupAsync" "Setup_Succeed" (E.encodeJson 4 args')
        with e ->
            setupError <- Some e
            logException env "App.setupAsync" "Setup_Failed" (E.encodeJson 4 args') e
    }
    new (loggingArgs : LoggingArgs, scope : Scope) =
        App (loggingArgs.CreateLogging (), scope)
    new (scope : Scope) =
        App (getLogging (), scope)
    member this.SetupAsync (getArgs : unit -> AppArgs) : Task<unit> = task {
        if args.IsSome then
            failWith "Already_Setup" <| E.encodeJson 4 args.Value
        else
            let args' = getArgs ()
            args <- Some args'
            do! setupAsync this
            match setupError with
            | None -> ()
            | Some e -> raise e
        return ()
        }
    member this.SetupAsync (args' : AppArgs) : Task<unit> =
        fun () -> args'
        |> this.SetupAsync
    member this.SetupAsync (args' : Json) : Task<unit> =
        fun () ->
            try
                castJson AppArgs.JsonDecoder args'
            with e ->
                logException env "App.SetupAsync" "Decode_Failed" args e
                raise e
        |> this.SetupAsync
    member this.SetupAsync (args' : string) : Task<unit> =
        let json : Json = parseJson args'
        this.SetupAsync json
    member __.SetupError : exn option = setupError
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : AppArgs = args |> Option.get
    interface ILogger with
        member __.Log m = env.Log m
    interface IPack with
        member __.Env : IEnv = env
    interface IDbPack with
        member this.Args = this.Args.AsDbPackArgs
        member __.FarangoDb (* IDbPack *) : FarangoDb.Agent = farangoDb |> Option.get
    member this.AsDbPack = this :> IDbPack
    interface ICloudHubPack with
        member this.Args = this.Args.AsCloudHubPackArgs
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
    member this.AsCloudHubPack = this :> ICloudHubPack
    interface IApp with
        member this.Args : AppArgs = this.Args
        member this.AsDbPack = this.AsDbPack
        member this.AsCloudHubPack = this.AsCloudHubPack
    member this.AsApp = this :> IApp