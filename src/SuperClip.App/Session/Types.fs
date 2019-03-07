module SuperClip.App.Session.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App

type ChannelService = SuperClip.Core.Channel.Service.Service
type CloudStub = IProxy<Cloud.Req, Cloud.ClientRes, Cloud.Evt>

type Args = NoArgs

and Model = {
    Auth : Credential option
    Channel : ChannelService option
    Syncing : bool
    mutable LastCloudItem : Item option
} with
    static member Empty =
        {
            Auth = None
            Channel = None
            Syncing = true
            LastCloudItem = None
        }

and Req =
    | DoSetAuth of Credential * Callback<unit>
    | DoResetAuth of Callback<unit>
    | DoSetSyncing of bool * Callback<unit>
with interface IReq

and Evt =
    | OnJoinSucceed of Cloud.JoinRes
    | OnJoinFailed of Reason<Cloud.JoinErr>
    | OnAuthSucceed of Cloud.AuthRes
    | OnAuthFailed of Reason<Cloud.AuthErr>
    | OnAuthChanged of Credential option
    | OnSyncingChanged of bool
    | OnDevicesChanged of Device list
with interface IEvt

and InternalEvt =
    | SetChannel of Cloud.AuthRes * ChannelService

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
