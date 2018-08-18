[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.View.Types
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let init (parts : Parts) : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        ({
            Parts = parts
        }, noCmd)

let update : Update<View, Model, Msg> =
    fun runner msg model ->
        match msg with
        | SetPrimary content ->
            model.Primary.Actor.Handle <| Clipboard.DoSet content None
            (model, noCmd)
        | HistoryEvt evt ->
            (model, noCmd)

let subscribe : Subscribe<View, Model, Msg> =
    fun runner model ->
        Cmd.batch [
            subscribeBus runner model HistoryEvt model.History.Actor.OnEvent
        ]

let getText (item : Item) =
    let text =
        match item.Content with
        | Text text -> text
    sprintf "[%A] %s" item.Time text

let render : Render =
    fun runner model ->
        View.ContentPage (
            content = View.StackLayout (padding=20.0,
                children =
                    (model.History.Actor.State.RecentItems
                    |> List.map (fun item ->
                        View.Label(text=getText item, horizontalOptions=LayoutOptions.Center, fontSize = "Large")
                    ))
                (*
                [
                    yield
                        View.StackLayout(padding=20.0, verticalOptions=LayoutOptions.Center,
                        children = [
                            View.Label(text=getText model.Primary, horizontalOptions=LayoutOptions.Center, fontSize = "Large")
                            View.Button(text="Get", command= (fun () -> dispatch Get))
                            View.Button(text="Set", command= (fun () -> dispatch (Set "test")))
                        ])
                    //yield View.Button(text="Reset", horizontalOptions=LayoutOptions.Center, command=fixf(fun () -> dispatch Reset), canExecute = (model <> initModel))
                ]
                *)
            )
        )

let args application (parts : Parts) =
    Args.Create (init parts) update subscribe render application