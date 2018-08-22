[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Widget.Items

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
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let render (runner : View) (items : Item list) : Widget list =
    items
    |> List.map ^<| Item.render runner