module SuperClip.Fabulous.View.Types

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Platform
open Dap.Gui
open Dap.Fabulous

module ViewTypes = Dap.Fabulous.View.Types

open SuperClip.Core
open SuperClip.App
module HistoryTypes = SuperClip.Core.History.Types

type Page =
    | NoPage
    | AuthPage
    | SettingsPage
    | DevicesPage

type HelpTopic =
    | HelpHome
    | HelpAuth
    | HelpSettings
    | HelpDevices

type InfoDialog = {
    Title : string
    Content : string
    DevInfo : string option
} with
    static member Create title content devInfo =
        {
            Title = title
            Content = content
            DevInfo = devInfo
        }

type Args = ViewTypes.Args<ISessionPack, Model, Msg>

and Model = {
    Resetting : bool
    Page : Page
    Info : InfoDialog option
    Help : HelpTopic option
    Ver : int
    mutable Password : string
}

and Msg =
    | SetPrimary of Content
    | HistoryEvt of HistoryTypes.Evt
    | DoSetResetting of bool
    | DoRepaint
    | DoDismissInfo
    | DoSetPage of Page
    | DoSetInfo of InfoDialog option
    | DoSetHelp of HelpTopic option
with interface IMsg

and View = ViewTypes.View<IApp, Model, Msg>
and Initer = ViewTypes.Initer<Model, Msg>
and Render = ViewTypes.Render<IApp, Model, Msg>
and Widget = ViewTypes.Widget
