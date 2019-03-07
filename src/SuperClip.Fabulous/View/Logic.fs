[<RequireQualifiedAccess>]
module SuperClip.Fabulous.View.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types
open Xamarin.Forms.PlatformConfiguration.iOSSpecific

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

type LayoutOptions = Xamarin.Forms.LayoutOptions

let private init : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        ({
            Page = NoPage
            Auth = AuthForm.Create None
            Info = None
            Help = None
            Ver = 1
        }, noCmd)

let private update : Update<View, Model, Msg> =
    fun runner msg model ->
        logWarn runner "View" "update" (msg, model)
        let model = {model with Ver = model.Ver + 1}
        match msg with
        | SetPrimary content ->
            runner.Pack.Primary.Actor.Handle <| Clipboard.DoSet content None
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
        | DoSetHelp help ->
            ({model with Help = help}, noCmd)
        | DoDismissInfo ->
            ({model with Info = None}, noCmd)

let private setInfoDialog (runner : View) title (kind, content, devInfo) =
    let title = sprintf "%s: %s" title kind
    let info = InfoDialog.Create title content devInfo
    runner.React <| DoSetInfo ^<| Some info

let private onSessionEvt (runner : View) (evt : SessionTypes.Evt) =
    match evt with
    | SessionTypes.OnJoinSucceed res ->
        runner.React <| DoSetPage NoPage
    | SessionTypes.OnAuthSucceed res ->
        runner.React <| DoSetPage NoPage
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
        View.NavigationPage (
            popped = (fun args ->
                //TODO: Remove hard-coded title check
                match args.Page.Title with
                | "Auth" ->
                    runner.React <| DoSetPage NoPage
                | "Settings" ->
                    runner.React <| DoSetPage NoPage
                | "Devices" ->
                    runner.React <| DoSetPage NoPage
                | "Help" ->
                    runner.React <| DoSetHelp None
                | "Error" ->
                    runner.React <| DoSetInfo None
                | _ -> ()
            ),
            pages = [
                yield Page.Home.render runner model
                match model.Page with
                | NoPage ->
                    ()
                | AuthPage ->
                    yield Page.Auth.render runner model
                | SettingsPage ->
                    yield Page.Settings.render runner model
                | DevicesPage ->
                    yield Page.Devices.render runner model
                if model.Help.IsSome then
                    yield Page.Help.render runner model.Help.Value
                if model.Info.IsSome then
                    yield Page.Info.render runner model.Info.Value
            ]
        )

let newArgs () =
    Args.Create init update subscribe render