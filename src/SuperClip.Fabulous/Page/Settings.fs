[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Settings

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

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let render (runner : View) (model : Model) =
    let view = View.NonScrollingContentPage (
        "Settings",
        [
            View.TableView (
                intent = TableIntent.Menu,
                items = [
                    ("Display", [
                        View.SwitchCell (
                            text = "Dark Theme",
                            on = Theme.isDark (),
                            onChanged = (fun args ->
                                Theme.setDark args.Value
                                runner.React DoReset
                            ),
                            created = Theme.decorate
                        )
                    ])
                ],
                created = Theme.decorate
            )
        ]
    )
    view.ToolbarItems([
        yield toolbarItem "Help" (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpSettings
        )
    ])
