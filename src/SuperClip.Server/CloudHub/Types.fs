[<AutoOpen>]
module SuperClip.Server.CloudHub.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Remote.Server.Auth

open SuperClip.Core
open SuperClip.Server

open SuperClip.Core.Cloud
module ChannelTypes = SuperClip.Core.Channel.Types

type Req = SuperClip.Core.Cloud.Types.ServerReq
type Evt = SuperClip.Core.Cloud.Types.Evt

type ChannelService = SuperClip.Core.Channel.Service.Service
type ChannelAuth = SuperClip.Server.Service.ChannelAuth.Record

and Args = App

and Model = {
    Devices : Map<string, Device>
    Channels : Map<string, ChannelService>
}

and InternalEvt =
    | OnDisconnected
    | AddChannel of ChannelService
    | RemoveChannel of ChannelService
    | AddDevice of Device
    | RemoveDevice of Device

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

type Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, Evt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)
