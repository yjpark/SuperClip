[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.About

open Xamarin.Forms

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

let render (runner : View) (model : Model) =
    v_box {
        children [
            label {
                text Locale.Text.About.Content
            }

            button {
                classId Theme.Button_Big
                text Locale.Text.Common.Ok
                command (fun _ ->
                    runner.React <| DoSetPage NoPage
                )
            }
        ]
    }|> contentPage Locale.Text.About.Title
