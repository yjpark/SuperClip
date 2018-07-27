module SuperClip.App.App

open FSharp.Control.Tasks.V2

open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
module HistoryTypes = SuperClip.Core.History.Types

module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service

let mutable history : History.Agent option = None

let setupAsync (env : IEnv) = task {
    let! history' = env |> Helper.doSetupAsync
    history <- Some history'
}

let init : Init<AppIniter, unit, AppModel, AppMsg> =
    fun initer () ->
        ({
            Primary = initer.Env |> Primary.get noKey
            History = history |> Option.get
        }, Cmd.none)

let update : Update<App, AppModel, AppMsg> =
    fun runner model msg ->
        match msg with
        | SetPrimary content ->
            model.Primary.Actor.Handle <| Clipboard.DoSet' content None
            (model, Cmd.none)
        | PrimaryEvt evt ->
            logError runner "Update" "PrimaryEvt" evt
            match evt with
            | Clipboard.OnChanged item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd' item None
            (model, Cmd.none)

let subscribe : Subscribe<App, AppModel, AppMsg> =
    fun runner model ->
        subscribeBus runner model PrimaryEvt model.Primary.Actor.OnEvent

let getText (item : Clipboard.Item) =
    let text =
        match item.Content with
        | Clipboard.Text text -> text
    sprintf "[%A] %s" item.Time text

let view : AppView =
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

let args = AppArgs.Create init update subscribe view setupAsync

let initApp consoleLogLevel logFile =
    App.CreateAsync args consoleLogLevel logFile
    |> Async.AwaitTask
    |> Async.RunSynchronously

let initApplication () =
    let app = initApp LogLevelError "super-clip-.log"
    app.Application