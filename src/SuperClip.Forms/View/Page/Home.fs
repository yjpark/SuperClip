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
open SuperClip.Forms.View.Types
open SuperClip.Forms.View.Widget
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let render (runner : View) (model : Model) : Widget =
    View.ScrollView (
        View.StackLayout (
            padding = 20.0,
            children = (
                match model.History.Actor.State.RecentItems with
                | [] ->
                    [
                        View.Label (
                            text = "No Recent Items"
                        )
                    ]
                | items ->
                    Items.render runner items
            )
        )
    )