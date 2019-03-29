[<RequireQualifiedAccess>]
module SuperClip.App.UserPref

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local

open SuperClip.Core
module HistoryTypes = SuperClip.Core.History.Types

[<Literal>]
let CredentialLuid = "credential"

[<Literal>]
let LocalHistoryLuid = "local_history"

[<Literal>]
let HistoryChangedSaveThreshold = 20

let setup (app : IApp) (runner : Context.Agent<IUserPref>) =
    let storage = IEnvironment.Instance.SecureStorage
    let historyChangedCount = runner.Context.Properties.HistoryChangedCount
    app.LocalHistory.Actor.OnEvent.AddWatcher runner "History_OnEvent" (fun evt ->
        match evt with
        | HistoryTypes.OnHistoryChanged ->
            historyChangedCount.SetValue (historyChangedCount.Value + 1)
    )
    historyChangedCount.OnChanged.AddWatcher runner "HistoryChangedCount_OnChanged" (fun count ->
        if count.New >= HistoryChangedSaveThreshold then
            runner.RunTask ignoreOnFailed (fun _ -> task {
                let text = encodeJson 4 app.LocalHistory.Actor.State
                let req = SetTextReq.Create (path = LocalHistoryLuid, text = text)
                do! storage.SetAsync.Handle req
                //logWarn runner "UserPref.setup" "LocalHistory_Saved" text
            })
            historyChangedCount.SetValue 0
    )
    runner.RunTask ignoreOnFailed (fun _ -> task {
        let! history = storage.GetAsync.Handle LocalHistoryLuid
        //logWarn runner "UserPref.setup" "LocalHistory_Loaded" history
        history
        |> Option.map (fun text ->
            decodeJson HistoryTypes.Model.JsonDecoder text
        )|> Option.iter (fun history ->
            app.LocalHistory.Post <| HistoryTypes.DoMerge (history, None)
        )
    })
    runner.Context.Properties.Credential.OnChanged.AddWatcher runner "DoSave" (fun c ->
        match c.New with
        | None ->
            storage.Remove.Handle CredentialLuid
            //logWarn runner "UserPref.setup" "Credential_Removed" ()
        | Some c ->
            runner.RunTask ignoreOnFailed (fun _ -> task {
                let text = encodeJson 4 c
                let req = SetTextReq.Create (path = CredentialLuid, text = text)
                do! storage.SetAsync.Handle req
                //logWarn runner "UserPref.setup" "Credential_Saved" text
            })
    )
    runner.RunTask ignoreOnFailed (fun _ -> task {
        let! credential = storage.GetAsync.Handle CredentialLuid
        //logWarn runner "UserPref.setup" "Credential_Loaded" credential
        let credential =
            credential
            |> Option.map (fun text ->
                decodeJson Credential.JsonDecoder text
            )
        runner.Context.Properties.Credential.SetValue credential
    })
