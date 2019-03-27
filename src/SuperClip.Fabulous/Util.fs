[<AutoOpen>]
module SuperClip.Fabulous.Util

open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Builder

open SuperClip.App
open SuperClip.Fabulous.View.Types
module SessionTypes = SuperClip.App.Session.Types

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

let setInfoDialog (runner : View) title (kind, content, devInfo) =
    let title = sprintf "%s: %s" title kind
    let info = InfoDialog.Create title content devInfo
    runner.React <| DoSetInfo ^<| Some info

let setupCloudMode (runner : View) =
    if GuiPrefs.getCloudMode runner then
        runner.Pack.Stub.Actor.Args.RetryDelay <- Some 5.0<second>
        Dap.Remote.WebSocketProxy.Proxy.doReconnect runner.Pack.Stub
    else
        runner.Pack.Stub.Actor.Args.RetryDelay <- None
        runner.Pack.Stub.Actor.State.Extra.Socket
        |> Option.iter (fun socket ->
            socket.Actor.Handle <| Dap.WebSocket.Client.Types.DoDisconnect None
        )

let setupSeparateSyncing (runner : View) =
    if not (GuiPrefs.getSeparateSyncing runner) then
        let session = runner.Pack.Session.Actor.State
        let syncingUp = session.SyncingUp
        let syncingDown = session.SyncingDown
        if syncingUp <> syncingDown then
            if syncingUp then
                runner.Pack.Session.Post <| SessionTypes.DoSetSyncingUp (false, None)
            if syncingDown then
                runner.Pack.Session.Post <| SessionTypes.DoSetSyncingDown (false, None)


