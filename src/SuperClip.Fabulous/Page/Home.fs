[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Home

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

    let view = View.NonScrollingContentPage (
        "Super Clip",
        [
            View.TableView (
                intent = TableIntent.Menu,
                items = [
                    yield
                        ("Cloud Link", Widget.Link.render runner session)
                    if history.PinnedItems.Length > 0 then
                        yield
                            ("Pinned Items",
                                history.PinnedItems
                                |> List.map ^<| Widget.Item.render runner current true
                            )
                    yield
                        ("Recent Items",
                            history.RecentItems
                            |> List.map ^<| Widget.Item.render runner current false
                        )
                ],
                created = Theme.decorate
            )
        ]
    )
    view.ToolbarItems ([
        yield toolbarItem "Settings" (fun () ->
            runner.React <| DoSetPage SettingsPage
        )
    ])
