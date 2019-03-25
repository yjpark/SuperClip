[<RequireQualifiedAccess>]
module SuperClip.Gui.Presenter.LinkStatus

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
open SuperClip.Gui.Prefab

type Prefab = ILinkStatus

type Presenter (prefab : Prefab, app : IApp) =
    inherit BasePresenter<IApp, Prefab> (prefab, app)
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
        | NoChannel ->
            setSessionText "Logging ..."
            setActionTextDisabled "" true
        | Joined (syncingUp, syncingDown) ->
            setSessionText "Joined"
            if syncingDown then
                setActionTextDisabled "Pause" false
            else
                setActionTextDisabled "Resume" false
    do (
        setLinkText "TEST"
        app.CloudStub.OnStatus.AddWatcher prefab "OnStatus" (fun status ->
            setLinkText <| sprintf "%A" status
            updateSessionAndAction ()
        )
        app.Session.Actor.OnEvent.AddWatcher prefab "OnSession" (fun evt ->
            updateSessionAndAction ()
        )
        prefab.Action.OnClick.OnEvent.AddWatcher prefab "OnAction" (fun evt ->
            match app.LinkSessionStatus with
            | NoAuth ->
                app.Gui.DoLogin.Handle ()
            | _ ->
                ()
        )
    )
