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
    inherit ILogger
    abstract Env : IEnv with get
    abstract Args : AppArgs with get
    inherit IDbPack
    inherit ICloudHubPack

type App (logging : ILogging, scope : Scope) =
    let env = Env.live MailboxPlatform logging scope
    let mutable args : AppArgs option = None
    let mutable setupError : exn option = None
    let mutable (* IDbPack *) farangoDb : FarangoDb.Agent option = None
    static member Create logging scope = new App (logging, scope)
    abstract member SetupExtrasAsync : unit -> Task<unit>
    default __.SetupExtrasAsync () = task {
        return ()
    }
    member this.SetupAsync (getArgs : unit -> AppArgs) : Task<unit> = task {
        if args.IsNone then
            let args' = getArgs ()
            args <- Some args'
            try
                let! (* IDbPack *) farangoDb' = env |> Env.addServiceAsync (FarangoDb.spec args'.FarangoDb) "FarangoDb" ""
                farangoDb <- Some farangoDb'
                do! env |> Env.registerAsync (Dap.WebSocket.Internal.Logic.spec (* ICloudHubPack *) args'.PacketConn) "PacketConn"
                do! env |> Env.registerAsync (SuperClip.Server.CloudHub.Logic.spec (* ICloudHubPack *) this.AsDbPack args'.CloudHub) "CloudHub"
                do! env |> Env.registerAsync (Dap.Remote.WebSocketGateway.Logic.spec (* ICloudHubPack *) args'.CloudHubGateway) "CloudHubGateway"
                do! this.SetupExtrasAsync ()
                logInfo env "App.SetupAsync" "Setup_Succeed" (E.encodeJson 4 args')
            with e ->
                setupError <- Some e
                logException env "App.SetupAsync" "Setup_Failed" (E.encodeJson 4 args') e
        else
            logError env "App.SetupAsync" "Already_Setup" (args, setupError, getArgs)
    }
    member this.Setup (callback : IApp -> unit) (getArgs : unit -> AppArgs) : IApp =
        if args.IsSome then
            failWith "Already_Setup" <| E.encodeJson 4 args.Value
        env.RunTask0 raiseOnFailed (fun _ -> task {
            do! this.SetupAsync getArgs
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
    interface IApp with
        member __.Env : IEnv = env
        member __.Args : AppArgs = args |> Option.get
    interface IDbPack with
        member __.Args = (Option.get args) .AsDbPackArgs
        member __.FarangoDb (* IDbPack *) : FarangoDb.Agent = farangoDb |> Option.get
    member this.AsDbPack = this :> IDbPack
    interface ICloudHubPack with
        member __.Args = (Option.get args) .AsCloudHubPackArgs
        member __.GetPacketConnAsync (key : Key) (* ICloudHubPack *) : Task<IAgent<PacketConn.Req, PacketConn.Evt> * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "PacketConn" key
            return (agent :?> IAgent<PacketConn.Req, PacketConn.Evt>, isNew)
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
    interface ILogger with
        member __.Log m = env.Log m
    member this.AsApp = this :> IApp