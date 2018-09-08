[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Page.Auth

open Xamarin.Forms
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.View.Types
open SuperClip.Forms.View.Widget
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types
module SessionTypes = SuperClip.Forms.Session.Types

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
    model.Parts.Session.Post <| SessionTypes.DoSetAuth (auth, None)

let render (runner : View) (model : Model) : Widget =
    let setAuth = runner.React << DoSetAuth
    View.ScrollingContentPage (
        "Auth",
        [
            View.Label (
                text = "User Name:"
            )
            View.Entry (
                text = "",
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
                text = "",
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
                            runner.React <| DoSetPage HomePage
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
                            runner.React <| DoSetPage HomePage
                        )
                    )
                ]
            )
        ]
    )
