[<RequireQualifiedAccess>]
module SuperClip.Presenter.LinkStatus

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App

type Prefab = SuperClip.Prefab.LinkStatus.Prefab

type Presenter (app : IApp, prefab : Prefab) =
    inherit BasePresenter<Prefab> (prefab)
    let setLinkText (v : string) =
        app.SetGuiValue (prefab.Link.Model.Text, v)
    let setSessionText (v : string) =
        app.SetGuiValue (prefab.Session.Model.Text, v)
    let setActionTextDisabled (text : string) (disabled : bool) =
        app.SetGuiValue (prefab.Action.Model.Text, text)
        app.SetGuiValue (prefab.Action.Model.Disabled, disabled)
    let updateSessionAndAction () =
        match app.LinkSessionStatus with
        | NoLink ->
            setSessionText ""
            setActionTextDisabled "Connect" true
        | NoAuth ->
            setSessionText ""
            setActionTextDisabled "Login" false
        | NoChannel auth ->
            setSessionText "Logging ..."
            setActionTextDisabled "" true
        | Syncing auth ->
            setSessionText "Syncing"
            setActionTextDisabled "Pause" false
        | Pausing auth ->
            setSessionText "Not Syncing"
            setActionTextDisabled "Resume" false
    do (
        app.CloudStub.OnStatus.AddWatcher prefab "OnStatus" (fun status ->
            setLinkText <| sprintf "%A" status
            updateSessionAndAction ()
        )
        app.Session.Actor.OnEvent.AddWatcher prefab "OnSession" (fun evt ->
            updateSessionAndAction ()
        )
        prefab.Action.OnClick.OnEvent.AddWatcher prefab "OnAction" (fun evt ->
            app.Gui.DoLogin.Handle ()
        )
    )