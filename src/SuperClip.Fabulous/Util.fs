[<AutoOpen>]
module SuperClip.Fabulous.Util

open Xamarin.Essentials
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Context
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

let toolbarItem (text' : string) (iconGlyph : string) (command' : unit -> unit) =
    if Theme.useToolbarItemIcon () then
        let icon' = IGuiApp.Instance.Theme.Param.Icons.GetCachedPathIfCached iconGlyph
        toolbar_item {
            text text'
            icon icon'
            command command'
        }
    else
        toolbar_item {
            text text'
            command command'
        }
