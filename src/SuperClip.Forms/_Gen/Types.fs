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
module Cloud = SuperClip.Core.Cloud
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module SecureStorage = Dap.Forms.Provider.SecureStorage
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
            ?device : Device,
            ?channel : Channel,
            ?passHash : string,
            ?cryptoKey : string,
            ?token : string
        ) : Credential =
        {
            Device = (* Credential *) device
                |> Option.defaultWith (fun () -> (Device.Default ()))
            Channel = (* Credential *) channel
                |> Option.defaultWith (fun () -> (Channel.Default ()))
            PassHash = (* Credential *) passHash
                |> Option.defaultWith (fun () -> "")
            CryptoKey = (* Credential *) cryptoKey
                |> Option.defaultWith (fun () -> "")
            Token = (* Credential *) token
                |> Option.defaultWith (fun () -> "")
        }
    static member Default () =
        Credential.Create (
            (Device.Default ()), (* Credential *) (* device *)
            (Channel.Default ()), (* Credential *) (* channel *)
            "", (* Credential *) (* passHash *)
            "", (* Credential *) (* cryptoKey *)
            "" (* Credential *) (* token *)
        )
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
    static member UpdateDevice ((* Credential *) update : Device -> Device) (this : Credential) =
        this |> Credential.SetDevice (update this.Device)
    static member UpdateChannel ((* Credential *) update : Channel -> Channel) (this : Credential) =
        this |> Credential.SetChannel (update this.Channel)
    static member UpdatePassHash ((* Credential *) update : string -> string) (this : Credential) =
        this |> Credential.SetPassHash (update this.PassHash)
    static member UpdateCryptoKey ((* Credential *) update : string -> string) (this : Credential) =
        this |> Credential.SetCryptoKey (update this.CryptoKey)
    static member UpdateToken ((* Credential *) update : string -> string) (this : Credential) =
        this |> Credential.SetToken (update this.Token)
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
 * Generated: <Class>
 *     IsFinal
 *)
type PrefProperties (owner : IOwner, key : Key) =
    inherit WrapProperties<PrefProperties, IComboProperty> ()
    let target = Properties.combo (owner, key)
    let credential = target.AddVar<(* PrefProperties *) Credential option> ((E.option Credential.JsonEncoder), (D.option Credential.JsonDecoder), "credential", None, None)
    do (
        target.SealCombo ()
        base.Setup (target)
    )
    static member Create (o, k) = new PrefProperties (o, k)
    static member Default () = PrefProperties.Create (noOwner, NoKey)
    static member AddToCombo key (combo : IComboProperty) =
        combo.AddCustom<PrefProperties> (PrefProperties.Create, key)
    override this.Self = this
    override __.Spawn (o, k) = PrefProperties.Create (o, k)
    override __.SyncTo t = target.SyncTo t.Target
    member __.Credential (* PrefProperties *) : IVarProperty<Credential option> = credential

type PrefContext = CustomContext<PrefProperties>

let spawnPrefContext (runner : IAgent) =
    CustomContext<PrefProperties> (runner.Env.Logging, runner.Ident.Kind, PrefProperties.Create)

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

type IClientPackArgs =
    inherit ICorePackArgs
    inherit ICloudStubPackArgs
    abstract CredentialSecureStorage : SecureStorage.Args<Credential> with get
    abstract Preferences : Context.Args<PrefContext> with get
    abstract AsCorePackArgs : ICorePackArgs with get
    abstract AsCloudStubPackArgs : ICloudStubPackArgs with get

type IClientPack =
    inherit IPack
    inherit ICorePack
    inherit ICloudStubPack
    abstract Args : IClientPackArgs with get
    abstract CredentialSecureStorage : SecureStorage.Service<Credential> with get
    abstract Preferences : Context.Agent<PrefContext> with get
    abstract AsCorePack : ICorePack with get
    abstract AsCloudStubPack : ICloudStubPack with get