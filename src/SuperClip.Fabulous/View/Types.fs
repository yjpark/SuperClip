module SuperClip.Fabulous.View.Types

open Fabulous.DynamicViews

open Dap.Platform
open Dap.Fabulous

module ViewTypes = Dap.Fabulous.View.Types

open SuperClip.Core
open SuperClip.App
module HistoryTypes = SuperClip.Core.History.Types

type Page =
    | HomePage
    | AuthPage

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

type AuthForm = {
    ChannelName : string
    DeviceName : string
    Password : string
} with
    static member Create (credential : Credential option) =
        credential
        |> Option.map (fun c ->
            {
                ChannelName = c.Channel.Name
                DeviceName = c.Device.Name
                Password = ""
            }
        )|> Option.defaultValue
            {
                ChannelName = ""
                DeviceName = getDeviceName ()
                Password = ""
            }

type Args = ViewTypes.Args<ISessionPack, Model, Msg>

and Model = {
    Page : Page
    Auth : AuthForm
    Info : InfoDialog option
    Ver : int
}

and Msg =
    | SetPrimary of Content
    | HistoryEvt of HistoryTypes.Evt
    | DoRepaint
    | DoDismissInfo
    | DoSetPage of Page
    | DoSetAuth of AuthForm
    | DoSetInfo of InfoDialog option
with interface IMsg

and View = ViewTypes.View<IApp, Model, Msg>
and Initer = ViewTypes.Initer<Model, Msg>
and Render = ViewTypes.Render<IApp, Model, Msg>
and Widget = ViewTypes.Widget

type Fabulous.DynamicViews.View with
    static member ScrollingContentPage(title, children) =
        View.ContentPage(title=title, content=View.ScrollView(View.StackLayout(padding=20.0, children=children) ), useSafeArea=true)

    static member NonScrollingContentPage(title, children, ?gestureRecognizers) =
        View.ContentPage(title=title, content=View.StackLayout(padding=20.0, children=children, ?gestureRecognizers=gestureRecognizers), useSafeArea=true)

