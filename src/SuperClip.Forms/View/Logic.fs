[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Logic

open FSharp.Control.Tasks.V2

open Elmish
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

type LayoutOptions = Xamarin.Forms.LayoutOptions

let private init (parts : Parts) : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        ({
            Parts = parts
        }, noCmd)

let private update : Update<View, Model, Msg> =
    fun runner msg model ->
        match msg with
        | SetPrimary content ->
            model.Primary.Actor.Handle <| Clipboard.DoSet content None
            (model, noCmd)
        | HistoryEvt evt ->
            (model, noCmd)

let private subscribe : Subscribe<View, Model, Msg> =
    fun runner model ->
        Cmd.batch [
            subscribeBus runner model HistoryEvt model.History.Actor.OnEvent
        ]

let private render : Render =
    fun runner model ->
        View.ContentPage (
            content = Page.Home.render runner model
        )

let args application (parts : Parts) =
    Args.Create (init parts) update subscribe render application