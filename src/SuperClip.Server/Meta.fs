module SuperClip.Server.Meta

open Dap.Context
open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.WebSocket.Meta
open Dap.Remote.Meta
open Dap.Remote.Meta.Net

[<Literal>]
let CloudHubKind = "CloudHub"

[<Literal>]
let CloudHubGatewayKind = "CloudHubGateway"

type M with
    static member cloudHub () =
        let alias = "CloudHubTypes", "SuperClip.Server.CloudHub.Types"
        let args = M.noArgs
        let type' = "CloudHubTypes.Agent"
        let spec = "SuperClip.Server.CloudHub.Logic.spec"
        M.agent (args, type', spec, kind = CloudHubKind, aliases = [alias])
    static member cloudHubGateway (?logTraffic : bool) =
        M.gateway (
            aliases = [("CloudHubTypes", "SuperClip.Server.CloudHub.Types")],
            reqEvt = "CloudHubTypes.Req, CloudHubTypes.Evt",
            hubSpec = "CloudHubTypes.HubSpec",
            ?logTraffic = logTraffic,
            kind = CloudHubGatewayKind
        )



