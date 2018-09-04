[<RequireQualifiedAccess>]
module SuperClip.Forms.Pref

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
open Dap.Forms.Provider
module JsonStorage = Dap.Local.Storage.Json.Service

open SuperClip.Core

[<Literal>]
let CredentialLuid = "credential"

type Credential = {
    Device : Device
    Channel : Channel
    PassHash : string
    CryptoKey : string
    Token : string
} with
    static member Create device channel passHash cryptoKey token = {
        Device = device
        Channel = channel
        PassHash = passHash
        CryptoKey = cryptoKey
        Token = token
    }
    static member JsonDecoder =
        D.decode Credential.Create
        |> D.required "device" Device.JsonDecoder
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "pass_hash" D.string
        |> D.required "crypto_key" D.string
        |> D.optional "token" D.string ""
    static member JsonEncoder (this : Credential) =
        E.object [
            "device", Device.JsonEncoder this.Device
            "channel", Channel.JsonEncoder this.Channel
            "pass_hash", E.string this.PassHash
            "crypto_key", E.string this.CryptoKey
            "token", E.string this.Token
        ]
    interface IJson with
        member this.ToJson () =
            Credential.JsonEncoder this
    member this.Self =
        Peer.Create this.Channel this.Device

type PrefProperties (owner : IOwner, key : Key) =
    inherit WrapProperties<PrefProperties, IComboProperty> ("PrefProperties")
    let target = Properties.combo owner key
    let credential = target.AddVar<Credential option> ("Option<Credential>", (E.option Credential.JsonEncoder), (D.option Credential.JsonDecoder), "credential", None)
    do (
        base.Setup (target)
    )
    static member Create o k = new PrefProperties (o, k)
    static member Empty () = PrefProperties.Create noOwner NoKey
    override this.Self = this
    override __.Spawn o k = PrefProperties.Create o k
    override __.SyncTo t = target.SyncTo t.Target
    member __.Credential : IVarProperty<Credential option> = credential

type State = CustomContext<PrefProperties>

let initAsync (env : IEnv) = task {
    SecureStorage.setSecret <| Des.encrypt "teiChe0xuo4maepezaihee8geigooTha" "mohtohJahmeechoch3sei3pheejaeGhu"
    let! storage = env |> SecureStorage.Service.addAsync<Credential> "Pref" 0 Credential.JsonEncoder Credential.JsonDecoder
    let pref = Context.custom<PrefProperties> "Pref" PrefProperties.Create
    let! credential = storage.PostAsync <| JsonStorage.TryLoad CredentialLuid
    credential
    |> Option.iter (fun c ->
        logWarn env "Pref" "Credential_Loaded" (E.encodeJson 4 c)
    )
    pref.Properties.Credential.SetValue credential |> ignore
    pref.Properties.Credential.OnValueChanged.AddWatcher storage "DoSave" (fun c ->
        match c.New with
        | None ->
            logWarn env "Pref" "Remove_Credential" ()
            SecureStorage.remove CredentialLuid
            |> ignore
        | Some c ->
            logWarn env "Pref" "Save_Credential" (E.encodeJson 4 c)
            storage.Post <| JsonStorage.DoSave CredentialLuid c None
    )
    return pref
}

let getDeviceName () =
    ""