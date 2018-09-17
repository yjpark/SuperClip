module SuperClip.Forms.Session.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
module History = SuperClip.Core.History.Agent
module CloudTypes = SuperClip.Core.Cloud.Types

type ChannelService = SuperClip.Core.Channel.Service.Service
type CloudStub = IProxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>

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
    | OnJoinSucceed of Join.Res
    | OnJoinFailed of Reason<Join.Err>
    | OnAuthSucceed of Auth.Res
    | OnAuthFailed of Reason<Auth.Err>
    | OnAuthChanged of Credential option
    | OnSyncingChanged of bool
with interface IEvt

and InternalEvt =
    | SetChannel of Auth.Res * ChannelService

and Msg =
    | Req of Req
    | Evt of Evt
    | PrimaryEvt of Clipboard.Evt
    | StubRes of CloudTypes.ClientRes
    | StubEvt of CloudTypes.Evt
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
