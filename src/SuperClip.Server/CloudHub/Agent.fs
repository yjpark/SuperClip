module SuperClip.Server.CloudHub.Agent

open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Platform
open Dap.Remote

module PacketConn = Dap.Remote.WebSocketService.PacketConn
module WebSocketServiceAgent = Dap.Remote.WebSocketService.Agent

open SuperClip.Core
open SuperClip.Server
open SuperClip.Server.CloudHub.Types
module CloudTypes = SuperClip.Core.Cloud.Types


[<Literal>]
let Kind = "CloudHub"

[<Literal>]
let ServiceKind = "CloudHubService"

type Agent = IAgent<Req, Evt>

let registerAsync app env =
    Logic.spec app
    |> fun spec -> env |> Env.registerAsync spec Kind

let registerServiceAsync (app : App) =
    let hubSpec =
        Hub.getHub<SuperClip.Server.CloudHub.Types.Agent, Req, Evt> app.Env Kind (fun runner ->
            runner.Deliver <| InternalEvt OnDisconnected
        )|> CloudTypes.getHubSpec
    WebSocketServiceAgent.registerAsync ServiceKind hubSpec true

let doRegisterAsync (app : App) = task {
    let env = app.Env
    do! env |> PacketConn.registerAsync true None
    do! env |> registerAsync app
    do! env |> registerServiceAsync app
}