[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Page.Auth

open Xamarin.Forms
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.View.Types
open SuperClip.Forms.View.Widget
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let render (runner : View) (model : Model) : Widget =
    View.StackLayout (
        padding = 20.0,
        children = (
            Items.render runner model.History.Actor.State.RecentItems
        )
    )
