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

let useToolbarItemIcon () =
    //iOS has align issue
    //Android generated empty images
    DeviceInfo.Platform = DevicePlatform.UWP
        || DeviceInfo.Platform = DevicePlatform.Android
    //    || DeviceInfo.Platform = DevicePlatform.iOS

let alwaysRefreshIcons = false //For testing

let ensureIcons () =
    if useToolbarItemIcon () then
        if alwaysRefreshIcons then
            Theme.icons.RefreshAll ()
        else
            Theme.icons.EnsureAll ()

let toolbarItem (text' : string) (iconGlyph : string) (command' : unit -> unit) =
    if useToolbarItemIcon () then
        let icon' = Theme.icons.GetCachedPathIfCached iconGlyph
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
