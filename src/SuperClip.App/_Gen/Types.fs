[<AutoOpen>]
module SuperClip.App.Types

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local
open SuperClip.Core

module Proxy = Dap.Remote.WebSocketProxy.Proxy
module Cloud = SuperClip.Core.Cloud
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module Context = Dap.Platform.Context

(*
 * Generated: <Record>
 *     IsJson
 *)
type Credential = {
    Device : (* Credential *) Device
    Channel : (* Credential *) Channel
    PassHash : (* Credential *) string
    CryptoKey : (* Credential *) string
    Token : (* Credential *) string
} with
    static member Create
        (
            ?device : (* Credential *) Device,
            ?channel : (* Credential *) Channel,
            ?passHash : (* Credential *) string,
            ?cryptoKey : (* Credential *) string,
            ?token : (* Credential *) string
        ) : Credential =
        {
            Device = (* Credential *) device
                |> Option.defaultWith (fun () -> (Device.Create ()))
            Channel = (* Credential *) channel
                |> Option.defaultWith (fun () -> (Channel.Create ()))
            PassHash = (* Credential *) passHash
                |> Option.defaultWith (fun () -> "")
            CryptoKey = (* Credential *) cryptoKey
                |> Option.defaultWith (fun () -> "")
            Token = (* Credential *) token
                |> Option.defaultWith (fun () -> "")
        }
    static member SetDevice ((* Credential *) device : Device) (this : Credential) =
        {this with Device = device}
    static member SetChannel ((* Credential *) channel : Channel) (this : Credential) =
        {this with Channel = channel}
    static member SetPassHash ((* Credential *) passHash : string) (this : Credential) =
        {this with PassHash = passHash}
    static member SetCryptoKey ((* Credential *) cryptoKey : string) (this : Credential) =
        {this with CryptoKey = cryptoKey}
    static member SetToken ((* Credential *) token : string) (this : Credential) =
        {this with Token = token}
    static member JsonEncoder : JsonEncoder<Credential> =
        fun (this : Credential) ->
            E.object [
                "device", Device.JsonEncoder (* Credential *) this.Device
                "channel", Channel.JsonEncoder (* Credential *) this.Channel
                "pass_hash", E.string (* Credential *) this.PassHash
                "crypto_key", E.string (* Credential *) this.CryptoKey
                "token", E.string (* Credential *) this.Token
            ]
    static member JsonDecoder : JsonDecoder<Credential> =
        D.object (fun get ->
            {
                Device = get.Required.Field (* Credential *) "device" Device.JsonDecoder
                Channel = get.Required.Field (* Credential *) "channel" Channel.JsonDecoder
                PassHash = get.Required.Field (* Credential *) "pass_hash" D.string
                CryptoKey = get.Required.Field (* Credential *) "crypto_key" D.string
                Token = get.Required.Field (* Credential *) "token" D.string
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Credential> (Credential.JsonEncoder, Credential.JsonDecoder)
    interface IJson with
        member this.ToJson () = Credential.JsonEncoder this
    interface IObj
    member this.WithDevice ((* Credential *) device : Device) =
        this |> Credential.SetDevice device
    member this.WithChannel ((* Credential *) channel : Channel) =
        this |> Credential.SetChannel channel
    member this.WithPassHash ((* Credential *) passHash : string) =
        this |> Credential.SetPassHash passHash
    member this.WithCryptoKey ((* Credential *) cryptoKey : string) =
        this |> Credential.SetCryptoKey cryptoKey
    member this.WithToken ((* Credential *) token : string) =
        this |> Credential.SetToken token

(*
 * Generated: <Combo>
 *)
type UserProps (owner : IOwner, key : Key) =
    inherit WrapProperties<UserProps, IComboProperty> ()
    let target' = Properties.combo (owner, key)
    let historyChangedCount = target'.AddVar<(* UserProps *) int> (E.int, D.int, "history_changed_count", 0, None)
    let credential = target'.AddVar<(* UserProps *) Credential option> ((E.option Credential.JsonEncoder), (D.option Credential.JsonDecoder), "credential", None, None)
    do (
        base.Setup (target')
    )
    static member Create (o, k) = new UserProps (o, k)
    static member Create () = UserProps.Create (noOwner, NoKey)
    static member AddToCombo key (combo : IComboProperty) =
        combo.AddCustom<UserProps> (UserProps.Create, key)
    override this.Self = this
    override __.Spawn (o, k) = UserProps.Create (o, k)
    override __.SyncTo t = target'.SyncTo t.Target
    member __.HistoryChangedCount (* UserProps *) : IVarProperty<int> = historyChangedCount
    member __.Credential (* UserProps *) : IVarProperty<Credential option> = credential

(*
 * Generated: <Context>
 *)
type IUserPref =
    inherit IContext<UserProps>
    abstract UserProps : UserProps with get

(*
 * Generated: <Context>
 *)
[<Literal>]
let UserPrefKind = "UserPref"

type UserPref (logging : ILogging) =
    inherit CustomContext<UserPref, ContextSpec<UserProps>, UserProps> (logging, new ContextSpec<UserProps>(UserPrefKind, UserProps.Create))
    static member Create (?logging : ILogging) =
        let logging = logging |> Option.defaultWith (fun () -> getLogging ())
        new UserPref (logging)
    override this.Self = this
    override __.Spawn l = new UserPref (l)
    member this.UserProps : UserProps = this.Properties
    interface IUserPref with
        member this.UserProps : UserProps = this.Properties
    member this.AsUserPref = this :> IUserPref

(*
 * Generated: <Pack>
 *)
type ICloudStubPackArgs =
    inherit ITickingPackArgs
    abstract CloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt> with get
    abstract PacketClient : PacketClient.Args with get
    abstract AsTickingPackArgs : ITickingPackArgs with get

type ICloudStubPack =
    inherit IPack
    inherit ITickingPack
    abstract Args : ICloudStubPackArgs with get
    abstract CloudStub : Proxy.Proxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt> with get
    abstract GetPacketClientAsync : Key -> Task<PacketClient.Agent * bool>
    abstract AsTickingPack : ITickingPack with get

(*
 * Generated: <Pack>
 *)
type IClientPackArgs =
    inherit ICorePackArgs
    inherit ICloudStubPackArgs
    abstract UserPref : Context.Args<IUserPref> with get
    abstract AsCorePackArgs : ICorePackArgs with get
    abstract AsCloudStubPackArgs : ICloudStubPackArgs with get

type IClientPack =
    inherit IPack
    inherit ICorePack
    inherit ICloudStubPack
    abstract Args : IClientPackArgs with get
    abstract UserPref : Context.Agent<IUserPref> with get
    abstract AsCorePack : ICorePack with get
    abstract AsCloudStubPack : ICloudStubPack with get

(*
 * Generated: <Context>
 *)
type IAppGui =
    inherit IFeature
    inherit IContext<NoProperties>
    abstract NoProperties : NoProperties with get
    abstract DoLogin : IHandler<unit, unit> with get

(*
 * Generated: <Context>
 *)
[<Literal>]
let AppGuiKind = "AppGui"

[<AbstractClass>]
type BaseAppGui<'context when 'context :> IAppGui> (logging : ILogging) =
    inherit CustomContext<'context, ContextSpec<NoProperties>, NoProperties> (logging, new ContextSpec<NoProperties>(AppGuiKind, NoProperties.Create))
    let doLogin = base.Handlers.Add<unit, unit> (E.unit, D.unit, E.unit, D.unit, "do_login")
    member this.NoProperties : NoProperties = this.Properties
    member __.DoLogin : IHandler<unit, unit> = doLogin
    interface IAppGui with
        member this.NoProperties : NoProperties = this.Properties
        member __.DoLogin : IHandler<unit, unit> = doLogin
    interface IFeature
    member this.AsAppGui = this :> IAppGui

(*
 * Generated: <Pack>
 *)
type IGuiPackArgs =
    abstract AppGui : Context.Args<IAppGui> with get

type IGuiPack =
    inherit IPack
    abstract Args : IGuiPackArgs with get
    abstract AppGui : Context.Agent<IAppGui> with get