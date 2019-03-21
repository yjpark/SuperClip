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
                runner.Pack.Stub.Status.ToString (), Theme.TextActionCell_NoLink, "", Some ("Login", fun _ ->
                    runner.React <| DoSetPage AuthPage
                )
            | Some auth, None ->
                "Logging in ...", Theme.TextActionCell_Linking, auth.Device.Name, None
            | Some auth, Some channel ->
                channel.Channel.Name, Theme.TextActionCell_Linked, auth.Device.Name, Some ("Logout", fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                )
        else
            let action =
                if stubStatus = LinkStatus.NoLink then
                    Some ("Connect", fun _ ->
                        Dap.Remote.WebSocketProxy.Proxy.doReconnect runner.Pack.Stub
                    )
                else
                    None
            runner.Pack.Stub.Status.ToString (), Theme.TextActionCell_NoLink, runner.Pack.Stub.Actor.Args.Uri, action
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
                let detail' = sprintf "Other Devices: %d" channel.Devices.Length
                yield text_action_cell {
                    text text'
                    detail detail'
                    action "Details"
                    onAction (fun _ ->
                        runner.React <| DoSetPage DevicesPage
                    )
                }
            let syncing = session.Syncing
            yield switch_cell {
                text "Sync with others"
                on syncing
                onChanged (fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoSetSyncing (not syncing, None)
                )
            }
    ]