[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
open SuperClip.Forms
open SuperClip.Forms.View.Types
module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

type LayoutOptions = Xamarin.Forms.LayoutOptions

let private init : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        ({
            Page = HomePage
            Auth = AuthForm.Create None
            Info = None
            Ver = 1
        }, noCmd)

let private update : Update<View, Model, Msg> =
    fun runner msg model ->
        logWarn runner "View" "update" (msg, model)
        let model = {model with Ver = model.Ver + 1}
        match msg with
        | SetPrimary content ->
            runner.Pack.Primary.Actor2.Handle <| Clipboard.DoSet content None
            (model, noCmd)
        | HistoryEvt _evt ->
            (model, noCmd)
        | DoRepaint ->
            (model, noCmd)
        | DoSetPage page ->
            ({model with Page = page}, noCmd)
        | DoSetAuth auth ->
            ({model with Auth = auth}, noCmd)
        | DoSetInfo info ->
            ({model with Info = info}, noCmd)
        | DoDismissInfo ->
            ({model with Info = None}, noCmd)

let private setInfoDialog (runner : View) title (kind, content, devInfo) =
    let title = sprintf "%s: %s" title kind
    let info = InfoDialog.Create title content devInfo
    runner.React <| DoSetInfo ^<| Some info

let private onSessionEvt (runner : View) (evt : SessionTypes.Evt) =
    match evt with
    | SessionTypes.OnJoinFailed reason ->
        Stub.getReasonContent reason
        |> setInfoDialog runner "Login Failed"
    | SessionTypes.OnAuthFailed reason ->
        Stub.getReasonContent reason
        |> setInfoDialog runner "Auth Failed"
    | _ ->
        runner.React DoRepaint

let private subscribe : Subscribe<View, Model, Msg> =
    fun runner model ->
        runner.Pack.CloudStub.OnStatus.AddWatcher runner "DoRepaint" (fun _status ->
            runner.React DoRepaint
        )
        runner.Pack.Session.Actor.OnEvent.AddWatcher runner "onSessionEvt" <| onSessionEvt runner
        Cmd.batch [
            subscribeBus runner model HistoryEvt runner.Pack.History.Actor.OnEvent
        ]

let private render : Render =
    fun runner model ->
        logWarn runner "View" "render" model.Page
        let page =
            match model.Page with
            | HomePage ->
                Page.Home.render runner model
            | AuthPage ->
                Page.Auth.render runner model
        if model.Info.IsSome then
            View.NavigationPage (
                pages = [
                    page
                    Dialog.Info.render runner (Option.get model.Info)
                ]
            )
        else
            page

let args application =
    Args.Create init update subscribe render application

let newArgs () =
    Dap.Forms.Fabulous.Util.newApplication ()
    |> args