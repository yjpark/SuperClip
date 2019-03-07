module SuperClip.Fabulous.Widget.Link

open Xamarin.Forms
open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform

open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types
module SessionTypes = SuperClip.App.Session.Types

let private button text command =
    View.Button (
        text = text,
        horizontalOptions = LayoutOptions.End,
        verticalOptions = LayoutOptions.Center,
        command = command
    )

let private addMenuItem (text : string) (command : unit -> unit) (v : TextCell) =
    let menuItem = new MenuItem (Text = text)
    menuItem.Clicked.Add (fun _ ->
        command ()
    )
    v.ContextActions.Add menuItem
    v

let render (runner : View) (session : SessionTypes.Model) =
    let text, color, detail =
        if runner.Pack.Stub.Status = LinkStatus.Linked then
            match session.Auth, session.Channel with
            | None, None ->
                runner.Pack.Stub.Status.ToString (), Color.Red, None
            | None, Some channel ->
                runner.Pack.Stub.Status.ToString (), Color.Red, Some channel.Channel.Name
            | Some auth, None ->
                "Logging in ...", Color.BlueViolet, Some auth.Device.Name
            | Some auth, Some channel ->
                channel.Channel.Name, Color.Green, Some auth.Device.Name
        else
            runner.Pack.Stub.Status.ToString (), Color.Red, Some runner.Pack.Stub.Actor.Args.Uri
    let actionText, actionCommand =
        if runner.Pack.Stub.Status = LinkStatus.Linked then
            match session.Auth with
                | Some auth ->
                    Some "Logout", Some (fun () ->
                        runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                    )
                | None ->
                    Some "Login", Some (fun () ->
                        runner.React <| DoSetPage AuthPage
                    )
        else
            None, None
    [
        yield View.TextActionCell (
            text = text,
            textColor = color,
            ?detail = detail,
            detailColor = Color.Gray,
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
                    textColor = Color.Black,
                    detail = detail,
                    detailColor = Color.Gray,
                    actionText = "Details",
                    actionCommand = ignore
                )
            let syncing = session.Syncing
            yield View.SwitchCell (
                text = "Sync with others",
                on = syncing,
                onChanged = (fun _ ->
                    runner.Pack.Session.Post <| SessionTypes.DoSetSyncing (not syncing, None)
                )
            )
    ]