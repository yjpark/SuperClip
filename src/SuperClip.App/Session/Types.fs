module SuperClip.App.Session.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App

type ChannelAgent = SuperClip.Core.Channel.Types.Agent
type CloudStub = IProxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt>

type Args = NoArgs

and Model = {
    Auth : Credential option
    Channel : ChannelAgent option
    SyncingUp : bool
    SyncingDown : bool
    mutable LastCloudItem : Item option
} with
    static member Empty =
        {
            Auth = None
            Channel = None
            SyncingUp = true
            SyncingDown = true
            LastCloudItem = None
        }

and Req =
    | DoSetAuth of Credential * Callback<unit>
    | DoResetAuth of Callback<unit>
    | DoSetSyncingUp of bool * Callback<unit>
    | DoSetSyncingDown of bool * Callback<unit>
with interface IReq

and Evt =
    | OnJoinSucceed of Cloud.JoinRes
    | OnJoinFailed of Reason<Cloud.JoinErr>
    | OnAuthSucceed of Cloud.AuthRes
    | OnAuthFailed of Reason<Cloud.AuthErr>
    | OnAuthChanged of Credential option
    | OnSyncingChanged
    | OnDevicesChanged of Device list
with interface IEvt

and InternalEvt =
    | SetChannel of Cloud.AuthRes * ChannelAgent

and Msg =
    | Req of Req
    | Evt of Evt
    | PrimaryEvt of Clipboard.Evt
    | StubRes of Cloud.ClientRes
    | StubEvt of Cloud.Evt
    | StubStatus of LinkStatus
    | InternalEvt of InternalEvt
with interface IMsg

and Agent (pack, param) =
    inherit PackAgent<IClientPack, Agent, Args, Model, Msg, Req, Evt> (pack, param)
    override this.Runner = this
    static member Spawn k m = new Agent (k, m)

let castEvt (msg : Msg) =
    match msg with
    | Evt evt -> Some evt
    | _ -> None
