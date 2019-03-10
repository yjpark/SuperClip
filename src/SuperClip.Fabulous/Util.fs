[<AutoOpen>]
module SuperClip.Fabulous.Util

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Platform
open Dap.Gui
open Dap.Fabulous


type Fabulous.DynamicViews.View with
    static member ScrollingContentPage(title, children) =
        View.ContentPage (
            title = title,
            content = View.ScrollView (
                View.StackLayout (
                    children = children
                )
            ),
            useSafeArea=true,
            created = Theme.decorate
        )

    static member NonScrollingContentPage(title, children, ?gestureRecognizers) =
        View.ContentPage (
            title = title,
            content = View.StackLayout(
                children = children,
                ?gestureRecognizers = gestureRecognizers
            ),
            useSafeArea=true,
            created = Theme.decorate
        )

    static member inline TextActionCell(
                                ?text: string,
                                ?textClassId: string,
                                ?detail: string,
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
                                    fontSize = 18,
                                    lineBreakMode = LineBreakMode.MiddleTruncation,
                                    ?classId = textClassId,
                                    created = Theme.decorate
                                )
                                View.Label (
                                    ?text = detail,
                                    fontSize = 12,
                                    lineBreakMode = LineBreakMode.MiddleTruncation,
                                    classId = Theme.Label_Dimmed,
                                    created = Theme.decorate
                                )
                            ],
                            created = Theme.decorate
                        )
                    else
                        yield View.Label (
                            ?text = text,
                            horizontalOptions = LayoutOptions.FillAndExpand,
                            verticalOptions = LayoutOptions.Center,
                            fontSize = 18,
                            lineBreakMode = LineBreakMode.MiddleTruncation,
                            ?classId = textClassId,
                            created = Theme.decorate
                        )
                    if actionText.IsSome then
                        yield View.Button (
                            ?text = actionText,
                            horizontalOptions = LayoutOptions.End,
                            verticalOptions = LayoutOptions.Center,
                            command = defaultArg actionCommand ignore,
                            created = Theme.decorate
                        )
                ],
                created = Theme.decorate
            )
        )

let toolbarItem (text : string) (command : unit -> unit) =
    View.ToolbarItem (
        text = text,
        //textColor = Theme.getValue (fun t -> t.Fabulous.Colors.Primary),
        command = command
    )