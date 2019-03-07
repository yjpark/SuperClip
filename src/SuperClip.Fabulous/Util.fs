[<AutoOpen>]
module SuperClip.Fabulous.Util

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Platform
open Dap.Fabulous

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

let toolbarItem (text : string) (command : unit -> unit) =
    View.ToolbarItem (
        text = text,
        command = command
    )