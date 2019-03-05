[<RequireQualifiedAccess>]
module SuperClip.App.UserPref

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local

open SuperClip.Core

[<Literal>]
let CredentialLuid = "credential"

let setup (storage : ISecureStorage) (runner : Context.Agent<IUserPref>) =
    runner.Context.Properties.Credential.OnChanged.AddWatcher runner "DoSave" (fun c ->
        match c.New with
        | None ->
            logWarn runner "UserPref.setup" "Remove_Credential" ()
            storage.Remove.Handle CredentialLuid
        | Some c ->
            let text = encodeJson 4 c
            logWarn runner "UserPref.setup" "Save_Credential" text
            runner.RunTask ignoreOnFailed (fun _ -> task {
                let req = SetTextReq.Create (path = CredentialLuid, text = text)
                do! storage.SetAsync.Handle req
            })
    )
    runner.RunTask ignoreOnFailed (fun _ -> task {
        let! credential = storage.GetAsync.Handle CredentialLuid
        logWarn runner "UserPref.setup" "Load_Credential" credential
        let credential =
            if System.String.IsNullOrEmpty credential then
                None
            else
                Some credential
            |> Option.map (fun text ->
                decodeJson Credential.JsonDecoder text
            )
        runner.Context.Properties.Credential.SetValue credential
    })
