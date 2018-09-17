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

let setup (storage : SecureStorage.Service<Credential>) (runner : Context.Agent<PrefContext>) =
    runner.Context.Properties.Credential.OnValueChanged.AddWatcher runner "DoSave" (fun c ->
        match c.New with
        | None ->
            logWarn runner "DoSave" "Remove_Credential" ()
            SecureStorage.remove CredentialLuid
            |> ignore
        | Some c ->
            logWarn runner "DoSave" "Save_Credential" (E.encodeJson 4 c)
            storage.Post <| JsonStorage.DoSave CredentialLuid c None
    )
    runner.RunTask ignoreOnFailed (fun _ -> task {
        let! credential = storage.PostAsync <| JsonStorage.TryLoad CredentialLuid
        credential
        |> Option.iter (fun c ->
            logWarn runner "Setup" "Credential_Loaded" (E.encodeJson 4 c)
        )
        runner.Context.Properties.Credential.SetValue credential |> ignore
    })
