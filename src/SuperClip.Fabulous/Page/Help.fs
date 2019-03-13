[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Help

open Xamarin.Forms

open Dap.Prelude
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
                text "TODO"
            }
            button {
                classId Theme.Button_Big
                text "Ok"
                command (fun _ ->
                    runner.React <| DoSetHelp None
                )
            }
        ]
    }|> scrollPage "Help"