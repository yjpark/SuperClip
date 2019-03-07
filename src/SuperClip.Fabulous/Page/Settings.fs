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
    let current = runner.Pack.Primary.Actor.State.Current
    let session = runner.Pack.Session.Actor.State
    let history = runner.Pack.History.Actor.State
    let view = View.ScrollingContentPage (
        "Settings",
        [
            View.TableView (
                intent = TableIntent.Menu,
                items = [
                    ("Display", [
                        View.SwitchCell (
                            text = "Dark Theme",
                            on = false,
                            onChanged = (fun _ ->
                                () //TODO
                                //runner.Pack.Session.Post <| SessionTypes.DoSetSyncing (not syncing, None)
                            )
                        )
                    ])
                ]
            )
        ]
    )
    view.ToolbarItems([
        yield toolbarItem "Help" (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpSettings
        )
    ])
