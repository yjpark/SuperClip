[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Resetting

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
        "Resetting",
        [
            View.Label (
                text = "Theme has been changed",
                created = Theme.decorate
            )
            View.Button (
                text = "Ok",
                horizontalOptions = LayoutOptions.FillAndExpand,
                verticalOptions = LayoutOptions.Center,
                command = (fun _ ->
                    runner.React <| DoSetResetting false
                ),
                classId = Theme.Button_Big,
                created = Theme.decorate
            )
        ]
    )