[<RequireQualifiedAccess>]
module SuperClip.Fabulous.View.Logic

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Gui
open Dap.Fabulous.Builder

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
        Theme.ensureIcons ()
        let runner = initer :?> View
        IGuiApp.Instance.SwitchTheme <| GuiPrefs.getTheme runner
        IGuiApp.Instance.SwitchLocale <| GuiPrefs.getLocale runner
        setupCloudMode runner
        ({
            Resetting = false
            Page = NoPage
            Info = None
            Help = None
            Ver = 1
            LoggingIn = false
            Password = ""
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
        | DoSetResetting v ->
            runner.Reset ()
            ({model with Resetting = v ; Page = NoPage}, noCmd)
        | DoRepaint ->
            (model, noCmd)
        | DoSetPage page ->
            ({model with Page = page}, noCmd)
        | DoSetInfo info ->
            ({model with Info = info}, noCmd)
        | DoSetHelp help ->
            ({model with Help = help}, noCmd)
        | DoDismissInfo ->
            ({model with Info = None}, noCmd)
        | DoSetLoggingIn v ->
            ({model with LoggingIn = v}, noCmd)

let private onSessionEvt (runner : View) (evt : SessionTypes.Evt) =
    match evt with
    | SessionTypes.OnJoinSucceed res ->
        if runner.ViewState.Page = AuthPage then
            runner.React <| DoSetPage NoPage
        else
            runner.React DoRepaint
    | SessionTypes.OnAuthSucceed res ->
        runner.React <| DoSetLoggingIn false
        if runner.ViewState.Page = AuthPage then
            runner.React <| DoSetPage NoPage
        else
            runner.React DoRepaint
    | SessionTypes.OnJoinFailed reason ->
        runner.React <| DoSetLoggingIn false
        Stub.getReasonContent reason
        |> setInfoDialog runner "Login Failed"
    | SessionTypes.OnAuthFailed reason ->
        runner.React <| DoSetLoggingIn false
        Stub.getReasonContent reason
        |> setInfoDialog runner "Auth Failed"
    | _ ->
        ()
    runner.React DoRepaint

let private subscribe : Subscribe<View, Model, Msg> =
    fun runner model ->
        runner.Pack.UserPref.Context.Properties.Credential.OnChanged.AddWatcher runner "OnCredential" (fun c ->
            match c.New with
            | None ->
                ()
            | Some c ->
                runner |> GuiPrefs.setAuthDevice c.Device.Name
                runner |> GuiPrefs.setAuthChannel c.Channel.Name
        )
        runner.Pack.CloudStub.OnStatus.AddWatcher runner "DoRepaint" (fun status ->
            if status = LinkStatus.Closed then
                runner.React <| DoSetLoggingIn false
            runner.React DoRepaint
        )
        runner.Pack.Session.Actor.OnEvent.AddWatcher runner "onSessionEvt" <| onSessionEvt runner
        batchCmd [
            subscribeBus runner model HistoryEvt runner.Pack.History.Actor.OnEvent
        ]

let private render : Render =
    fun runner model ->
        if model.Resetting then
            Page.Resetting.render runner model
        else
            navigation_page {
                pages [
                    //yield Page.About.render runner model
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
                popped (fun args ->
                    //TODO: Remove hard-coded title check
                    let title = args.Page.Title
                    if title = Locale.Text.Auth.Title then
                        runner.React <| DoSetPage NoPage
                    elif title = Locale.Text.Settings.Title then
                        runner.React <| DoSetPage NoPage
                    elif title = Locale.Text.Devices.Title then
                        runner.React <| DoSetPage NoPage
                    elif title = Locale.Text.Help.Title then
                        runner.React <| DoSetHelp None
                    elif title = Locale.Text.Error.Title then
                        runner.React <| DoSetInfo None
                )
            }

let newArgs () =
    Args.Create init update subscribe render