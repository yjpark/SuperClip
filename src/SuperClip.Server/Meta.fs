module SuperClip.Server.Meta

open Dap.Context
open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.WebSocket.Meta
open Dap.Remote.Meta

[<Literal>]
let CloudHubKind = "CloudHub"

[<Literal>]
let CloudHubGatewayKind = "CloudHubGateway"

type M with
    static member cloudHubSpawner () =
        let alias = "CloudHubTypes", "SuperClip.Server.CloudHub.Types"
        let args = M.noArgs
        let type' = "CloudHubTypes.Agent"
        let spec = "SuperClip.Server.CloudHub.Logic.spec"
        M.spawner ([alias], args, type', spec, CloudHubKind)
    static member cloudHubGatewaySpawner () =
        M.gatewaySpawner (
            [("CloudHubTypes", "SuperClip.Server.CloudHub.Types")],
            "CloudHubTypes.Req, CloudHubTypes.Evt", "CloudHubTypes.HubSpec",
            true, CloudHubGatewayKind)



