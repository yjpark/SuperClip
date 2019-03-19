[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Settings

open Xamarin.Forms
open Fabulous.DynamicViews

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

let render (runner : View) (model : Model) =
    table_view {
        intent TableIntent.Settings
        items [
            ("Display", [
                switch_cell {
                    text "Dark Theme"
                    on (Theme.isDark ())
                    onChanged (fun args ->
                        Theme.setDark args.Value
                        runner.React <| DoSetResetting true
                    )
                }
            ])
        ]
    }|> contentPage "Settings"
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem "Help" Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpSettings
            )
        ])
    )