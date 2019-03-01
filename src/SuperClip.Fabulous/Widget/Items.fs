[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Widget.Items

open Xamarin.Forms
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous.View.Types
module HistoryTypes = SuperClip.Core.History.Types

let render (runner : View) (items : Item list) : Widget =
    View.ScrollView (
        View.StackLayout (
            padding = 20.0,
            children = (
                items
                |> List.map ^<| Item.render runner
            )
        )
    )