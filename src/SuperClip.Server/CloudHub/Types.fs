[<AutoOpen>]
module SuperClip.Server.CloudHub.Types

open Dap.Prelude
open Dap.Platform
open Dap.Local.Farango
open Dap.Remote
open Dap.Remote.Server.Auth

open SuperClip.Core
open SuperClip.Server

module ChannelTypes = SuperClip.Core.Channel.Types

[<Literal>]
let Kind = "CloudHub"

[<Literal>]
let GatewayKind = "CloudHubGateway"

type Req = Cloud.ServerReq
type Evt = Cloud.Evt

type ChannelService = SuperClip.Core.Channel.Service.Service
type ChannelAuth = SuperClip.Server.Service.ChannelAuth.Record

and Args = NoArgs

and Model = {
    Devices : Map<ChannelKey, ChannelService * Device>
}

and InternalEvt =
    | OnStatusChanged of LinkStatus
    | AddDevice of ChannelService * Device
    | RemoveDevice of ChannelKey * Device

and Msg =
    | HubReq of Req
    | HubEvt of Evt
    | InternalEvt of InternalEvt
    | ChannelEvt of ChannelService * ChannelTypes.Evt
with interface IMsg

let castEvt : CastEvt<Msg, Evt> =
    function
    | HubEvt evt -> Some evt
    | _ -> None

type Agent (pack, param) =
    inherit PackAgent<IDbPack, Agent, Args, Model, Msg, Req, Evt> (pack, param)
    override this.Runner = this
    static member Spawn k m = new Agent (k, m)

let setGateway (gateway : IGateway) : Func<Agent, unit> =
    fun runner ->
        gateway.OnStatus.AddWatcher runner "OnStatus" (runner.Deliver << InternalEvt << OnStatusChanged)

let HubSpec =
    Hub.getHubSpec<Agent, Req, Evt> Kind Cloud.ServerReq.HubSpec setGateway
