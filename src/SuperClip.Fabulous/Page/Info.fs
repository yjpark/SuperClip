module SuperClip.Fabulous.Page.Info

open Xamarin.Forms
open Elmish
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform

open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

let render (runner : View) (info : InfoDialog) =
    View.ScrollingContentPage (
        "Error",
        [
            yield View.Label (
                text = info.Title,
                fontSize = "Large",
                created = Theme.decorate
            )
            yield View.Label (
                text = info.Content,
                created = Theme.decorate
            )
            if info.DevInfo.IsSome then
                yield View.Label (
                    text = info.DevInfo.Value,
                    created = Theme.decorate
                )
            yield View.Button (
                text = "Ok",
                horizontalOptions = LayoutOptions.Center,
                verticalOptions = LayoutOptions.Center,
                command = (fun () ->
                    runner.React <| DoSetInfo None
                ),
                created = Theme.decorate
            )
        ]
    )