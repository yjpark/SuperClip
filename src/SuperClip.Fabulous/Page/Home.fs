[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Home

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
    let current = runner.Pack.Primary.Actor.State.Current
    let session = runner.Pack.Session.Actor.State
    let history = runner.Pack.History.Actor.State

    table_view {
        intent TableIntent.Menu
        items [
            if GuiPrefs.getCloudMode () then
                yield
                    (Locale.Text.Home.CloudLink, Widget.Link.render runner session)
            if history.PinnedItems.Length > 0 then
                yield
                    (Locale.Text.Home.PinnedItems,
                        history.PinnedItems
                        |> List.map ^<| Widget.Item.render runner current true
                    )
            yield
                (Locale.Text.Home.RecentItems,
                    history.RecentItems
                    |> List.map ^<| Widget.Item.render runner current false
                )
        ]
    }|> scrollPage Locale.Text.Home.Title
    |> (fun view ->
        view.ToolbarItems ([
            yield toolbarItem Locale.Text.Settings.Title Icons.Settings (fun () ->
                runner.React <| DoSetPage SettingsPage
            )
            yield toolbarItem Locale.Text.Help.Title Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpHome
            )
        ])
    )
