module SuperClip.Forms.View.Dialog.Info

open Xamarin.Forms
open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform

open SuperClip.App
open SuperClip.Forms
open SuperClip.Forms.View.Types

let render (runner : View) (info : InfoDialog) =
    View.ScrollingContentPage (
        "Error",
        [
            yield View.Label (
                text = info.Title,
                fontSize = "Large"
            )
            yield View.Label (
                text = info.Content
            )
            if info.DevInfo.IsSome then
                yield View.Label (
                    text = info.DevInfo.Value
                )
            yield View.Button (
                text = "Ok",
                horizontalOptions = LayoutOptions.Center,
                verticalOptions = LayoutOptions.Center,
                command = (fun () ->
                    runner.React <| DoSetInfo None
                )
            )
        ]
    )