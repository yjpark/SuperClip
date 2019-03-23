[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Resetting

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
                text Locale.Text.Resetting.ThemeChanged
            }
            button {
                classId Theme.Button_Big
                text "Ok"
                command (fun _ ->
                    runner.React <| DoSetResetting false
                )
            }
        ]
    }|> contentPage Locale.Text.Resetting.Title
