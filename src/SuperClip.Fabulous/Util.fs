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
