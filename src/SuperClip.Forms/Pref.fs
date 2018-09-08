[<RequireQualifiedAccess>]
module SuperClip.Forms.Pref

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Forms
open Dap.Forms.Provider
module JsonStorage = Dap.Local.Storage.Json.Service

open SuperClip.Core

[<Literal>]
let CredentialLuid = "credential"

type Credential with
    member this.Self =
        Peer.Create this.Channel this.Device

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
