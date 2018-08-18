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

let device = Device.New "test"

let cryptoKey = calcCryptoKey "test"

let init (parts : Parts) : Init<Initer, unit, Model, Msg> =
    fun initer () ->
        let channel = Channel.New "test"
        let peer = Peer.Create channel device
        let req = Join.Req.Create peer <| calcPassHash "test"
        parts.CloudStub.Post <| CloudTypes.DoJoin req
        ({
            Primary = parts.Primary
            History = parts.History
            CloudStub = parts.CloudStub
            CloudPeers = None
        }, noCmd)

let doSetItemToCloud (item : Item) (model : Model) (runner : View) =
    model.CloudPeers
    |> Option.iter (fun peers ->
        let peer = Peer.Create peers.Channel device
        let item = item.ForCloud peer cryptoKey
        model.CloudStub.Post <| CloudTypes.DoSetItem item
    )

let update : Update<View, Model, Msg> =
    fun runner msg model ->
        match msg with
        | SetPrimary content ->
            model.Primary.Actor.Handle <| Clipboard.DoSet content None
            (model, noCmd)
        | PrimaryEvt evt ->
            logError runner "Update" "PrimaryEvt" evt
            match evt with
            | Clipboard.OnSet item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd item None
                runner |> doSetItemToCloud item model
            | Clipboard.OnChanged item ->
                model.History.Actor.Handle <| HistoryTypes.DoAdd item None
                runner |> doSetItemToCloud item model
            (model, noCmd)
        | CloudRes res ->
            logError runner "Update" "CloudRes" res
            match res with
            | CloudTypes.OnJoin (_req, (Ok token)) ->
                model.CloudStub.Post <| CloudTypes.DoAuth token
                (model, noCmd)
            | CloudTypes.OnAuth (_req, (Ok peers)) ->
                (runner, model, noCmd)
                |=|> updateModel (fun m -> {m with CloudPeers = Some peers})
            | _ ->
                (model, noCmd)
        | CloudEvt evt ->
            logError runner "Update" "CloudEvt" evt
            match evt with
            | CloudTypes.OnItemChanged item ->
                let item = item.Decrypt runner cryptoKey
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

let args application (parts : Parts) =
    Args.Create (init parts) update subscribe render application