[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Auth

open Xamarin.Forms
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

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

let render (runner : View) (model : Model) : Widget =
    let setAuth = runner.React << DoSetAuth
    let view = View.ScrollingContentPage (
        "Auth",
        [
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
                    setAuth {model.Auth with ChannelName = text}
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
    )
    view.HasNavigationBar(true).HasBackButton(false) |> ignore
    view.ToolbarItems ([
        yield toolbarItem "Help" (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpAuth
        )
    ])

