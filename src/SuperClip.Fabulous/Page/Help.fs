[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Help

open Xamarin.Forms

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let render (runner : View) (topic : HelpTopic) =
    v_box {
        children [
            label {
                text Locale.Text.Help.Home
            }
            label {
                text Locale.Text.Help.Settings
            }
            label {
                text Locale.Text.Help.Auth
            }
            label {
                text Locale.Text.Help.Devices
            }
            button {
                classId Theme.Button_Big
                text Locale.Text.Common.Ok
                command (fun _ ->
                    runner.React <| DoSetHelp None
                )
            }
        ]
    }|> scrollPage Locale.Text.Help.Title