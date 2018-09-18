[<AutoOpen>]
module SuperClip.Forms.Types

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open SuperClip.Core

module Proxy = Dap.Remote.WebSocketProxy.Proxy
module CloudTypes = SuperClip.Core.Cloud.Types
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module SecureStorage = Dap.Forms.Provider.SecureStorage

(*
 * Generated: <Record>
 *     IsJson
 *)
type Credential = {
    Device : Device
    Channel : Channel
    PassHash : string
    CryptoKey : string
    Token : string
} with
    static member Create device channel passHash cryptoKey token
            : Credential =
        {
            Device = device
            Channel = channel
            PassHash = passHash
            CryptoKey = cryptoKey
            Token = token
        }
    static member JsonEncoder : JsonEncoder<Credential> =
        fun (this : Credential) ->
            E.object [
                "device", Device.JsonEncoder this.Device
                "channel", Channel.JsonEncoder this.Channel
                "pass_hash", E.string this.PassHash
                "crypto_key", E.string this.CryptoKey
                "token", E.string this.Token
            ]
    static member JsonDecoder : JsonDecoder<Credential> =
        D.decode Credential.Create
        |> D.required "device" Device.JsonDecoder
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "pass_hash" D.string
        |> D.required "crypto_key" D.string
        |> D.required "token" D.string
    static member JsonSpec =
        FieldSpec.Create<Credential>
            Credential.JsonEncoder Credential.JsonDecoder
    interface IJson with
        member this.ToJson () = Credential.JsonEncoder this
    interface IObj
    member this.WithDevice (device : Device) = {this with Device = device}
    member this.WithChannel (channel : Channel) = {this with Channel = channel}
    member this.WithPassHash (passHash : string) = {this with PassHash = passHash}
    member this.WithCryptoKey (cryptoKey : string) = {this with CryptoKey = cryptoKey}
    member this.WithToken (token : string) = {this with Token = token}

(*
 * Generated: <Class>
 *     IsFinal
 *)
type PrefProperties (owner : IOwner, key : Key) =
    inherit WrapProperties<PrefProperties, IComboProperty> ()
    let target = Properties.combo owner key
    let credential = target.AddVar<Credential option> ((E.option Credential.JsonEncoder), (D.option Credential.JsonDecoder), "credential", None, None)
    do (
        target.SealCombo ()
        base.Setup (target)
    )
    static member Create o k = new PrefProperties (o, k)
    static member Default () = PrefProperties.Create noOwner NoKey
    static member AddToCombo key (combo : IComboProperty) =
        combo.AddCustom<PrefProperties>(PrefProperties.Create, key)
    override this.Self = this
    override __.Spawn o k = PrefProperties.Create o k
    override __.SyncTo t = target.SyncTo t.Target
    member __.Credential : IVarProperty<Credential option> = credential

type PrefContext = CustomContext<PrefProperties>

let spawnPrefContext (runner : IAgent) =
    CustomContext<PrefProperties> (runner.Env.Logging, runner.Ident.Kind, PrefProperties.Create)

type ICloudStubPackArgs =
    abstract CloudStub : Proxy.Args<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt> with get
    abstract PacketClient : PacketClient.Args with get

type ICloudStubPack =
    inherit ILogger
    abstract Env : IEnv with get
    abstract Args : ICloudStubPackArgs with get
    abstract CloudStub : Proxy.Proxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt> with get
    abstract GetPacketClientAsync : Key -> Task<PacketClient.Agent * bool>

type IClientPackArgs =
    inherit ICorePackArgs
    inherit ICloudStubPackArgs
    abstract CredentialSecureStorage : SecureStorage.Args<Credential> with get
    abstract Preferences : Context.Args<PrefContext> with get

type IClientPack =
    inherit ILogger
    inherit ICorePack
    inherit ICloudStubPack
    abstract Env : IEnv with get
    abstract Args : IClientPackArgs with get
    abstract CredentialSecureStorage : SecureStorage.Service<Credential> with get
    abstract Preferences : Context.Agent<PrefContext> with get