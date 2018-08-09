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
open SuperClip.Forms
open SuperClip.Forms.View.Types
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let init (parts : Parts) : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        ({
            Primary = parts.Primary
            History = parts.History
            CloudStub = parts.CloudStub
        }, noCmd)

let update : Update<View, Model, Msg> =
    fun runner model msg ->
        match msg with
        | SetPrimary content ->
            model.Primary.Actor.Handle <| Clipboard.DoSet content None
            (model, noCmd)
        | PrimaryEvt evt ->
            logError runner "Update" "PrimaryEvt" evt
            match evt with
            | Clipboard.OnSet item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd item None
            | Clipboard.OnChanged item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd item None
            (model, noCmd)
        | CloudRes res ->
            logError runner "Update" "CloudRes" res
            (model, noCmd)
        | CloudEvt evt ->
            logError runner "Update" "CloudEvt" evt
            match evt with
            | CloudTypes.OnChanged item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd item None
            | _ -> ()
            (model, noCmd)

let subscribe : Subscribe<View, Model, Msg> =
    fun runner model ->
        Cmd.batch [
            subscribeBus runner model PrimaryEvt model.Primary.Actor.OnEvent
            subscribeBus runner model CloudRes model.CloudStub.OnResponse
            subscribeBus runner model CloudEvt model.CloudStub.Actor.OnEvent
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

let args (parts : Parts) application =
    Args.Create (init parts) update subscribe render application