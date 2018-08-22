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
module SessionService = SuperClip.Forms.Session.Service

let initAsync (app : Simple.Model) = task {
    let! history = app.Env |> Helper.doSetupAsync
    let primary = app.Env |> PrimaryService.get NoKey
    do! app.Env |> PacketClient.registerAsync true None
    let! cloudStub = app.Env |> WebSocketProxy.addAsync NoKey CloudTypes.StubSpec CloudServerUri (Some 5.0<second>) true
    let args : SessionService.Args =
        {
            Stub = cloudStub
            Primary = primary
            History = history
        }
    let! session = app.Env |> SessionService.addAsync NoKey args
    return {
        Primary = primary
        History = history
        Session = session
    }
}
