[<AutoOpen>]
module SuperClip.Fabulous.Util

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Platform
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Builder

let contentPage (title' : string) (content' : ViewElement) =
    content_page {
        title title'
        useSafeArea true
        content content'
    }

let scrollPage (title' : string) (content' : ViewElement) =
    scroll_view {
        content content'
    }|> contentPage title'

let toolbarItem (text : string) (command : unit -> unit) =
    View.ToolbarItem (
        text = text,
        //textColor = Theme.getValue (fun t -> t.Fabulous.Colors.Primary),
        command = command
    )

type Xamarin.Forms.View with
    static member inline TextActionCell
        (
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
            ?actionCommand : unit -> unit) =
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
                                    ?classId = textClassId
                                )
                                View.Label (
                                    ?text = detail,
                                    fontSize = 12,
                                    lineBreakMode = LineBreakMode.MiddleTruncation
                                )
                            ]
                        )
                    else
                        yield View.Label (
                            ?text = text,
                            horizontalOptions = LayoutOptions.FillAndExpand,
                            verticalOptions = LayoutOptions.Center,
                            fontSize = 18,
                            lineBreakMode = LineBreakMode.MiddleTruncation,
                            ?classId = textClassId
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
