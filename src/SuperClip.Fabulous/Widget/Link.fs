module SuperClip.Fabulous.Widget.Link

open Xamarin.Forms

open Dap.Prelude
open Dap.Platform
open Dap.Fabulous
open Dap.Fabulous.Builder

open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types
module SessionTypes = SuperClip.App.Session.Types

let private addMenuItem (text : string) (command : unit -> unit) (v : TextCell) =
    let menuItem = new MenuItem (Text = text)
    menuItem.Clicked.Add (fun _ ->
        command ()
    )
    v.ContextActions.Add menuItem
    v

let render (runner : View) (session : SessionTypes.Model) =
    let text', classId', detail', action' =
        let stubStatus = runner.Pack.Stub.Status
        if stubStatus = LinkStatus.Linked then
            match session.Auth, session.Channel with
            | None, _ ->
                runner.Pack.Stub.Status.ToString (), Theme.TextCell_NoLink, "", Some (Locale.Text.Link.Login, fun _ ->
                    runner.React <| DoSetPage AuthPage
                )
            | Some auth, None ->
                Locale.Text.Link.LoggingIn, Theme.TextCell_Linking, auth.Device.Name, None
            | Some auth, Some channel ->
                channel.Channel.Name, Theme.TextCell_Linked, auth.Device.Name, Some (Locale.Text.Link.Logout, fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                )
        else
            let action =
                if stubStatus = LinkStatus.NoLink then
                    Some (Locale.Text.Link.Connect, fun _ ->
                        Dap.Remote.WebSocketProxy.Proxy.doReconnect runner.Pack.Stub
                    )
                else
                    None
            runner.Pack.Stub.Status.ToString (), Theme.TextCell_NoLink, runner.Pack.Stub.Actor.Args.Uri, action
    [
        yield
            match action' with
            | Some (actionText, actionCommand) ->
                text_action_cell {
                    classId classId'
                    text text'
                    detail detail'
                    action actionText
                    onAction actionCommand
                }
            | None ->
                text_cell {
                    classId classId'
                    text text'
                    detail detail'
                }
        if runner.Pack.Stub.Status = LinkStatus.Linked && session.Channel.IsSome then
            if session.Channel.IsSome then
                let channel = session.Channel.Value.Actor.State
                let text' =
                    channel.Devices
                    |> List.map (fun d -> sprintf "'%s'" d.Name)
                    |> String.concat " "
                let detail' = System.String.Format (Locale.Text.Link.OtherDevices, channel.Devices.Length)
                yield text_action_cell {
                    text text'
                    detail detail'
                    action Locale.Text.Link.Details
                    onAction (fun _ ->
                        runner.React <| DoSetPage DevicesPage
                    )
                }
            let syncingUp = session.SyncingUp
            yield switch_cell {
                text Locale.Text.Link.SyncingUp
                on syncingUp
                onChanged (fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoSetSyncingUp (not syncingUp, None)
                )
            }
            let syncingDown = session.SyncingDown
            yield switch_cell {
                text Locale.Text.Link.SyncingDown
                on syncingDown
                onChanged (fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoSetSyncingDown (not syncingDown, None)
                )
            }
    ]