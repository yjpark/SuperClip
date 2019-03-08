module SuperClip.Fabulous.Widget.Link

open Xamarin.Forms
open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Fabulous

open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types
module SessionTypes = SuperClip.App.Session.Types

let private button text command =
    View.Button (
        text = text,
        horizontalOptions = LayoutOptions.End,
        verticalOptions = LayoutOptions.Center,
        command = command,
        created = Theme.decorate
    )

let private addMenuItem (text : string) (command : unit -> unit) (v : TextCell) =
    let menuItem = new MenuItem (Text = text)
    menuItem.Clicked.Add (fun _ ->
        command ()
    )
    v.ContextActions.Add menuItem
    v

let render (runner : View) (session : SessionTypes.Model) =
    let text, color, detail, actionText, actionCommand =
        if runner.Pack.Stub.Status = LinkStatus.Linked then
            match session.Auth, session.Channel with
            | None, None ->
                runner.Pack.Stub.Status.ToString (), Color.Red, None, Some "Login", Some (fun () ->
                    runner.React <| DoSetPage AuthPage
                )
            | None, Some channel ->
                runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                runner.Pack.Stub.Status.ToString (), Color.Red, Some channel.Channel.Name, None, None
            | Some auth, None ->
                "Logging in ...", Color.BlueViolet, Some auth.Device.Name, None, None
            | Some auth, Some channel ->
                channel.Channel.Name, Color.Green, Some auth.Device.Name, Some "Logout", Some (fun () ->
                    runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                )
        else
            runner.Pack.Stub.Status.ToString (), Color.Red, Some runner.Pack.Stub.Actor.Args.Uri, None, None
    [
        yield View.TextActionCell (
            text = text,
            ?detail = detail,
            ?actionText = actionText,
            ?actionCommand = actionCommand
        )
        if runner.Pack.Stub.Status = LinkStatus.Linked && session.Channel.IsSome then
            if session.Channel.IsSome then
                let channel = session.Channel.Value.Actor.State
                let text =
                    channel.Devices
                    |> List.map (fun d -> sprintf "'%s'" d.Name)
                    |> String.concat " "
                let detail = sprintf "Other Devices: %d" channel.Devices.Length
                yield View.TextActionCell (
                    text = text,
                    detail = detail,
                    actionText = "Details",
                    actionCommand = (fun _ ->
                        runner.React <| DoSetPage DevicesPage
                    )
                )
            let syncing = session.Syncing
            yield View.SwitchCell (
                text = "Sync with others",
                on = syncing,
                onChanged = (fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoSetSyncing (not syncing, None)
                ),
                created = Theme.decorate
            )
    ]