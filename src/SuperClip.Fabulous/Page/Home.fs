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

let render (runner : View) (model : Model) =
    View.ContentPage (
        content = View.StackLayout (
            padding = 20.0,
            children = [
                yield Widget.Link.render runner runner.Pack.Session
                yield Widget.Items.render runner runner.Pack.History.Actor.State.RecentItems
            ]
        )
    )