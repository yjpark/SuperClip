module SuperClip.Forms.View.Widget.Link

open Xamarin.Forms
open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

open Dap.Prelude
open Dap.Platform

open SuperClip.App
open SuperClip.Forms
open SuperClip.Forms.View.Types
module SessionTypes = SuperClip.App.Session.Types

let private button text command =
    View.Button (
        text = text,
        horizontalOptions = LayoutOptions.End,
        verticalOptions = LayoutOptions.Center,
        command = command
    )

let render (runner : View) (session : SessionTypes.Agent) =
    View.FlexLayout (
        direction=FlexDirection.Row,
        children = [
            yield View.Label (
                text = sprintf "Link Status : %A" runner.Pack.Stub.Status
            )
            if runner.Pack.Stub.Status = LinkStatus.Linked then
                match session.Actor.State.Auth with
                | Some auth ->
                    let statusLabel = fun status ->
                        View.Label (sprintf "[%s] %s (%s)" status auth.Channel.Name auth.Device.Name)
                    match session.Actor.State.Channel with
                    | Some channel ->
                        match session.Actor.State.Syncing with
                        | true ->
                            yield statusLabel "Syncing"
                            yield button "Pause" (fun () ->
                                session.Post <| SessionTypes.DoSetSyncing (false, None)
                            )
                            yield button "Logout" (fun () ->
                                session.Post <| SessionTypes.DoResetAuth None
                            )
                        | false ->
                            yield statusLabel "Not Syncing"
                            yield button "Resume" (fun () ->
                                session.Post <| SessionTypes.DoSetSyncing (true, None)
                            )
                        (*
                        yield button "Logout" (fun () ->
                            Pref.clearCredential ()
                            runner.React <| DoRepaint
                        )
                        *)
                    | None ->
                        yield statusLabel "Logging On Server"
                | None ->
                    yield button "Login" (fun () ->
                        runner.React <| DoSetPage AuthPage
                    )
        ]
    )