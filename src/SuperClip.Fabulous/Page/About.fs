[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.About

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

let render (runner : View) (model : Model) =
    View.NonScrollingContentPage (
        "About",
        [
            View.Button (
                text = "Ok",
                horizontalOptions = LayoutOptions.CenterAndExpand,
                verticalOptions = LayoutOptions.Center,
                command = (fun _ ->
                    runner.React <| DoSetResetting false
                ),
                created = Theme.decorate
            )
        ]
    )