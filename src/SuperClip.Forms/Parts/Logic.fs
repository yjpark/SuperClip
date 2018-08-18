[<RequireQualifiedAccess>]
module SuperClip.Forms.Parts.Logic

open FSharp.Control.Tasks.V2

open Elmish
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms
open Plugin.Clipboard

open Dap.Local.App
open Dap.Prelude
open Dap.Platform
open Dap.Remote
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module WebSocketProxy = Dap.Remote.WebSocketProxy.Proxy

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.Parts.Types
module PrimaryService = SuperClip.Core.Primary.Service
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let private device = Device.New "test"

let private cryptoKey = calcCryptoKey "test"

let mutable private cloudPeers : Peers option = None

let private doSetItemToCloud (app : Simple.Model) (model : Model) (item : Item) =
    let runner = app.Env
    cloudPeers
    |> Option.iter (fun peers ->
        let peer = Peer.Create peers.Channel device
        let item = item.ForCloud peer cryptoKey
        model.CloudStub.Post <| CloudTypes.DoSetItem item
    )

let private onPrimaryEvt (app : Simple.Model) (model : Model) (evt : Clipboard.Evt) =
    let runner = app.Env
    logWarn runner "Parts" "PrimaryEvt" evt
    match evt with
    | Clipboard.OnSet item ->
        model.History.Actor.Handle <| HistoryTypes.DoAdd item None
        doSetItemToCloud app model item
    | Clipboard.OnChanged item ->
        model.History.Actor.Handle <| HistoryTypes.DoAdd item None
        doSetItemToCloud app model item

let private onCloudStubEvt (app : Simple.Model) (model : Model) (evt : CloudTypes.Evt) =
    let runner = app.Env
    logWarn runner "Parts" "CloudEvt" evt
    match evt with
    | CloudTypes.OnItemChanged item ->
        let item = item.Decrypt runner cryptoKey
        model.History.Actor.Handle <| HistoryTypes.DoAdd item None
    | _ -> ()

let private onCloudStubRes (app : Simple.Model) (model : Model) (res : CloudTypes.ClientRes) =
    let runner = app.Env
    logWarn runner "Parts" "CloudRes" res
    match res with
    | CloudTypes.OnJoin (_req, (Ok token)) ->
        model.CloudStub.Post <| CloudTypes.DoAuth token
    | CloudTypes.OnAuth (_req, (Ok peers)) ->
        cloudPeers <- Some peers
    | _ ->
        ()

let private setup (app : Simple.Model) (model : Model) =
    model.Primary.Actor.OnEvent.AddWatcher app.Env "Parts" <| onPrimaryEvt app model
    model.CloudStub.Actor.OnEvent.AddWatcher app.Env "Parts" <| onCloudStubEvt app model
    model.CloudStub.OnResponse.AddWatcher app.Env "Parts" <| onCloudStubRes app model
    let channel = Channel.New "test"
    let peer = Peer.Create channel device
    let req = Join.Req.Create peer <| calcPassHash "test"
    model.CloudStub.Post <| CloudTypes.DoJoin req
    model

let initAsync (app : Simple.Model) = task {
    let! history = app.Env |> Helper.doSetupAsync
    let primary = app.Env |> PrimaryService.get NoKey
    do! app.Env |> PacketClient.registerAsync true None
    let! cloudStub = app.Env |> WebSocketProxy.addAsync NoKey CloudTypes.StubSpec CloudServerUri true
    return {
        Primary = primary
        History = history
        CloudStub = cloudStub
    }|> setup app
}
