module SuperClip.Forms.App

open FSharp.Control.Tasks.V2

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Forms.App
module AppHelper = Dap.Forms.App.Helper

open SuperClip.Core
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module WebSocketProxy = Dap.Remote.WebSocketProxy.Proxy

module HistoryTypes = SuperClip.Core.History.Types
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types

let mutable history : History.Agent option = None
let mutable cloudStub : CloudStub option = None

let setupAsync (env : IEnv) = task {
    let! history' = env |> Helper.doSetupAsync
    history <- Some history'
    do! env |> PacketClient.registerAsync true None
    let! cloudStub' = env |> WebSocketProxy.addAsync NoKey CloudTypes.StubSpec CloudUri true
    let cloudStub' = cloudStub' :> CloudStub
    cloudStub <- Some cloudStub'
}

let init : Init<AppIniter, unit, AppModel, AppMsg> =
    fun initer () ->
        ({
            Primary = initer.Env |> Primary.get NoKey
            History = history |> Option.get
            CloudStub = cloudStub |> Option.get
        }, noCmd)

let update : Update<App, AppModel, AppMsg> =
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

let subscribe : Subscribe<App, AppModel, AppMsg> =
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

let mutable private env' : IEnv option = None
let getEnv () = env' |> Option.get

let mutable private onInit' : Bus<NoEvt> option = None
let getOnInit () = (onInit' |> Option.get) .Publish

let mutable private app' : App option = None
let getApp () = app' |> Option.get

let mutable private application' : Application option = None
let getApplication () = application' |> Option.get

let initApp application env =
    env' <- Some env
    application' <- Some application
    onInit' <- Some <| new Bus<NoEvt> (env :> IOwner)
    let args = AppArgs.Create init update subscribe view setupAsync application
    env
    |> AppHelper.init App.Spawn args (fun app ->
        app' <- Some app
        (onInit' |> Option.get) .Trigger NoEvt
    )

let initApplication () =
    let env = AppHelper.env "SuperClip" LogLevelError "super-clip-.log"
    env
    |> initApp (AppHelper.newApplication env |> Option.get)
    getApplication ()