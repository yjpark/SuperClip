module SuperClip.Forms.Session.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open Dap.Archive.WebSocket.Accessor.Types
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types

type ChannelService = SuperClip.Core.Channel.Service.Service
type CloudStub = IProxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>

type Args = {
    Stub : CloudStub
    Primary : Primary.Service
    History : History.Agent
}

and Model = {
    Auth : Pref.Credential option
    Self : Peer option
    Channel : ChannelService option
    mutable LastCloudItem : Item option
}

and Req =
    | DoSetAuth of Pref.Credential * Callback<unit>
with interface IReq

and Evt =
    | OnJoinSucceed of Join.Res
    | OnJoinFailed of Reason<Join.Error>
    | OnAuthFailed of Reason<Auth.Error>
with interface IEvt

and InternalEvt =
    | DoInit
    | SetChannel of ChannelService

and Msg =
    | Req of Req
    | Evt of Evt
    | PrimaryEvt of Clipboard.Evt
    | StubRes of CloudTypes.ClientRes
    | StubEvt of CloudTypes.Evt
    | InternalEvt of InternalEvt
with interface IMsg

and Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, Evt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)

let castEvt (msg : Msg) =
    match msg with
    | Evt evt -> Some evt
    | _ -> None
