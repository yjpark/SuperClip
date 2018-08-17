[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Forms.App

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Local.App
open Dap.Forms
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module WebSocketProxy = Dap.Remote.WebSocketProxy.Proxy
module WithView = Dap.Forms.App.WithView

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types
module ViewTypes = SuperClip.Forms.View.Types
module ViewLogic = SuperClip.Forms.View.Logic

[<Literal>]
let Scope = "SuperClip"

type ViewModel = ViewTypes.Model
type ViewMsg = ViewTypes.Msg
type View = ViewTypes.View

type Args = WithView.Args<ViewModel, ViewMsg, Parts>

type Model = WithView.Model<ViewModel, ViewMsg, Parts>

let mutable private instance : Model option = None
let getInstance () = instance |> Option.get

let onApp (onAppStarted : Model -> unit) (app : Model) =
    instance <- Some app
    onAppStarted app

let init (args : Args) (onApp : Model -> unit) =
    WithView.init<ViewModel, ViewMsg, Parts> args onApp

let newPartsAsync (app : Simple.Model) = task {
    let! history = app.Env |> Helper.doSetupAsync
    let primary = app.Env |> Primary.get NoKey
    do! app.Env |> PacketClient.registerAsync true None
    let! cloudStub = app.Env |> WebSocketProxy.addAsync NoKey CloudTypes.StubSpec CloudServerUri true
    return {
        Primary = primary
        History = history
        CloudStub = cloudStub
    }
}

let newViewAsync (application) (parts : Parts) =
    ViewLogic.args application parts
    |> WithView.newViewAsync

let create' consoleLogLevel logFile onAppStarted application =
    let args = Args.Create (Simple.Args.Default Scope consoleLogLevel logFile) newPartsAsync (newViewAsync application)
    init args (onApp onAppStarted)

let create onAppStarted =
    newApplication ()
    |> create' LogLevelWarning "super-clip-.log" onAppStarted

let createApplication () =
    let application = newApplication ()
    application
    |> create' LogLevelWarning "super-clip-.log" ignore
    |> ignore
    application