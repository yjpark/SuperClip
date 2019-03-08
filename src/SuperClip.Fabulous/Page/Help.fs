[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Help

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

let render (runner : View) (topic : HelpTopic) =
    View.ScrollingContentPage (
        "Help",
        [
            View.Label (
                text = "TODO",
                created = Theme.decorate
            )
        ]
    )
