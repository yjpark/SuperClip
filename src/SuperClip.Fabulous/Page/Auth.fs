[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Auth

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let doAuth (runner : View) (model : Model) : unit =
    let password = model.Auth.Password
    let auth : Credential =
        {
            Device = Device.New model.Auth.DeviceName
            Channel = Channel.CreateWithName model.Auth.ChannelName
            PassHash = calcPassHash password
            CryptoKey = calcCryptoKey password
            Token = ""
        }
    runner.Pack.Session.Post <| SessionTypes.DoSetAuth (auth, None)

let render' (runner : View) (model : Model) : Widget =
    let setAuth = runner.React << DoSetAuth
    let view =
        v_box {
            children [
                View.Label (
                    text = "User Name:"
                )
                View.Entry (
                    text = model.Auth.ChannelName,
                    horizontalOptions = LayoutOptions.FillAndExpand,
                    textChanged = (fun args ->
                        setAuth {model.Auth with ChannelName = args.NewTextValue}
                    ),
                    completed = (fun text ->
                        setAuth {model.Auth with ChannelName = text}
                    )
                )
                View.Label (
                    text = "Device Name:"
                )
                View.Entry (
                    text = model.Auth.DeviceName,
                    horizontalOptions = LayoutOptions.FillAndExpand,
                    textChanged = (fun args ->
                        setAuth {model.Auth with DeviceName = args.NewTextValue}
                    ),
                    completed = (fun text ->
                        setAuth {model.Auth with DeviceName = text}
                    )
                )
                View.Label (
                    text = "Password:"
                )
                View.Entry (
                    text = "",
                    isPassword = true,
                    horizontalOptions = LayoutOptions.FillAndExpand,
                    textChanged = (fun args ->
                        setAuth {model.Auth with Password = args.NewTextValue}
                    ),
                    completed = (fun text ->
                        setAuth {model.Auth with Password = text}
                    )
                )
                View.Label (
                    text = ""
                )
                View.FlexLayout (
                    direction=FlexDirection.Row,
                    children = [
                        View.Button (
                            text = "Cancel",
                            horizontalOptions = LayoutOptions.Center,
                            verticalOptions = LayoutOptions.Center,
                            command = (fun () ->
                                runner.React <| DoSetPage NoPage
                            )
                        )
                        View.Label (
                            text = "    "
                        )
                        View.Button (
                            text = "Login",
                            horizontalOptions = LayoutOptions.Center,
                            verticalOptions = LayoutOptions.Center,
                            command = (fun () ->
                                doAuth runner model
                                //TODO: Show mask to block user interaction
                            )
                        )
                    ]
                )
            ]
        }|> scrollPage "Auth"
    view.HasNavigationBar(true).HasBackButton(false) |> ignore
    view.ToolbarItems ([
        yield toolbarItem "Help" Icons.Help (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpAuth
        )
    ])

let render (runner : View) (model : Model) : Widget =
    let setAuth = runner.React << DoSetAuth
    v_box {
        children [
            label {
                text "Device"
            }
            entry {
                text model.Auth.DeviceName
                textChanged (fun args ->
                    setAuth {model.Auth with DeviceName = args.NewTextValue}
                )
                completed (fun text ->
                    setAuth {model.Auth with DeviceName = text}
                )
            }
            label {
                text "Channel"
            }
            entry {
                text model.Auth.ChannelName
                keyboard Keyboard.Email
                textChanged (fun args ->
                    setAuth {model.Auth with ChannelName = args.NewTextValue}
                )
                completed (fun text ->
                    setAuth {model.Auth with ChannelName = text}
                )
            }
            label {
                text "Password"
            }
            entry {
                text ""
                isPassword true
                textChanged (fun args ->
                    setAuth {model.Auth with Password = args.NewTextValue}
                )
                completed (fun text ->
                    setAuth {model.Auth with Password = text}
                )
            }
            label {
                text ""
            }
            button {
                classId Theme.Button_Big
                text "Login"
                command (fun () ->
                    doAuth runner model
                )
            }
        ]
    }|> scrollPage "Auth"
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem "Help" Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpAuth
            )
        ])
    )
