[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Page.Home

open Xamarin.Forms
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.View
open SuperClip.Forms.View.Types
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let render (runner : View) (model : Model) =
    View.ContentPage (
        content = View.StackLayout (
            padding = 20.0,
            children = [
                yield Widget.Link.render runner model.Parts.Session
                yield Widget.Items.render runner model.Parts.History.Actor.State.RecentItems
            ]
        )
    )