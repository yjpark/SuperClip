[<AutoOpen>]
module SuperClip.Forms.View.Types

open Xamarin.Forms
open Elmish.XamarinForms.DynamicViews

open Dap.Platform
open Dap.Remote

module ViewTypes = Dap.Forms.View.Types

open SuperClip.Core
open SuperClip.Forms
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types
module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.Forms.Session.Types

type Session = SessionTypes.Agent
type History = HistoryTypes.Agent

type Parts = SuperClip.Forms.Parts.Types.Model

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
    static member Create (credential : Pref.Credential option) =
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
                DeviceName = Pref.getDeviceName ()
                Password = ""
            }

type Args = ViewTypes.Args<Model, Msg>

and Model = {
    Parts : Parts
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

and View = ViewTypes.View<Model, Msg>
and Initer = ViewTypes.Initer<Model, Msg>
and Render = ViewTypes.Render<Model, Msg>
and Widget = ViewTypes.Widget

type Elmish.XamarinForms.DynamicViews.View with
    static member ScrollingContentPage(title, children) =
        View.ContentPage(title=title, content=View.ScrollView(View.StackLayout(padding=20.0, children=children) ), useSafeArea=true)

    static member NonScrollingContentPage(title, children, ?gestureRecognizers) =
        View.ContentPage(title=title, content=View.StackLayout(padding=20.0, children=children, ?gestureRecognizers=gestureRecognizers), useSafeArea=true)

