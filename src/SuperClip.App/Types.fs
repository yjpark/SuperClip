module SuperClip.App.Types

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Platform

open SuperClip.Core

type Model = {
    Primary : Clipboard.Item
}

type Msg =
    | Get
    | Set of string
    | OnPrimaryChanged of Clipboard.Item

let (env, primary) = Helper.initLocal "super-clip-.log"

let initModel = { Primary = Clipboard.Item.Empty }

let init () = initModel, Cmd.none

let update msg model =
    match msg with
    | Get ->
        primary.Actor.Handle <| Clipboard.DoGet None
        (model, Cmd.none)
    | Set text ->
        let content = Clipboard.Text text
        primary.Actor.Handle <| Clipboard.DoSet' content None
        (model, Cmd.none)
    | OnPrimaryChanged item ->
        ({model with Primary = item}, Cmd.none)

let getText (item : Clipboard.Item) =
    let text =
        match item.Content with
        | Clipboard.Text text -> text
    sprintf "%A [%i] %s" item.Time item.Index text

let view (model: Model) dispatch =
    View.ContentPage(
        content=View.StackLayout(padding=20.0,
            children=[
                yield
                    View.StackLayout(padding=20.0, verticalOptions=LayoutOptions.Center,
                    children=[
                        View.Label(text=getText model.Primary, horizontalOptions=LayoutOptions.Center, fontSize = "Large")
                        View.Button(text="Get", command= (fun () -> dispatch Get))
                        View.Button(text="Set", command= (fun () -> dispatch (Set "test")))
                    ])
                //yield View.Button(text="Reset", horizontalOptions=LayoutOptions.Center, command=fixf(fun () -> dispatch Reset), canExecute = (model <> initModel))
            ]
        )
    )

let subscribe _model =
    let sub = fun dispatch ->
        primary.Actor.OnEvent.AddWatcher primary "OnPrimaryChanged" (fun evt ->
            match evt with
            | Clipboard.OnChanged item ->
                dispatch <| OnPrimaryChanged item
        )
    Cmd.ofSub sub

type App () as app =
    inherit Application ()

    let program =
        Program.mkProgram init update view
        |> Program.withSubscription subscribe
    let runner =
        program
        |> Program.runWithDynamicView app
