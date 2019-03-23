module SuperClip.Fabulous.Page.Info

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Fabulous.Builder

open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

let render (runner : View) (info : InfoDialog) =
    v_box {
        children [
            yield label {
                text info.Title
                fontSize (makeFontSize "Large")
            }
            yield label {
                text info.Content
            }
            if info.DevInfo.IsSome then
                yield label {
                    text info.DevInfo.Value
                }
            yield button {
                classId Theme.Button_Big
                text Locale.Text.Common.Ok
                command (fun () ->
                    runner.React <| DoSetInfo None
                )
            }
        ]
    }|> scrollPage Locale.Text.Error.Title
