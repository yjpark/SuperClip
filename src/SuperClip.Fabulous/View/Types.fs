module SuperClip.Fabulous.View.Types

open Xamarin.Forms
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

    static member inline TextActionCell(
                                ?text: string,
                                ?detail: string,
                                ?textColor: Xamarin.Forms.Color,
                                ?detailColor: Xamarin.Forms.Color,
                                ?command: unit -> unit,
                                ?canExecute: bool,
                                ?commandParameter: System.Object,
                                ?height: double,
                                ?isEnabled: bool,
                                ?classId: string,
                                ?styleId: string,
                                ?automationId: string,
                                ?created: (Xamarin.Forms.TextCell -> unit),
                                ?ref: ViewRef<Xamarin.Forms.TextCell>,
                                ?actionText : string,
                                ?actionCommand : unit -> unit
                                ) =
        View.ViewCell (
            view = View.StackLayout (
                orientation = StackOrientation.Horizontal,
                padding = 0.0,
                spacing = 5.0,
                children = [
                    if detail.IsSome then
                        yield View.StackLayout (
                            orientation = StackOrientation.Vertical,
                            horizontalOptions = LayoutOptions.FillAndExpand,
                            padding = 0.0,
                            spacing = 5.0,
                            gestureRecognizers = [
                                View.TapGestureRecognizer (
                                    command = defaultArg command ignore
                                )
                            ],
                            children = [
                                View.Label (
                                    ?text = text,
                                    ?textColor = textColor,
                                    fontSize = 18,
                                    lineBreakMode = LineBreakMode.MiddleTruncation
                                )
                                View.Label (
                                    ?text = detail,
                                    fontSize = 12,
                                    textColor = defaultArg detailColor Color.Gray,
                                    lineBreakMode = LineBreakMode.MiddleTruncation
                                )
                            ]
                        )
                    else
                        yield View.Label (
                            ?text = text,
                            ?textColor = textColor,
                            horizontalOptions = LayoutOptions.FillAndExpand,
                            verticalOptions = LayoutOptions.Center,
                            fontSize = 18,
                            lineBreakMode = LineBreakMode.MiddleTruncation
                        )
                    if actionText.IsSome then
                        yield View.Button (
                            ?text = actionText,
                            horizontalOptions = LayoutOptions.End,
                            verticalOptions = LayoutOptions.Center,
                            command = defaultArg actionCommand ignore
                        )
                ]
            )
        )
