[<AutoOpen>]
module SuperClip.Server.CloudHub.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Server

module CloudTypes = SuperClip.Core.Cloud.Types

type Req = CloudTypes.ServerReq
type Evt = CloudTypes.Evt

and Args = App

and Session = {
    mutable Token : string //Token.Record option
    // mutable UserAuth : UserAuth.Record option
}

and Model = {
    Session : Session
} with
    member this.WithSession (session : Session) = {this with Session = session}

and InternalEvt =
    | OnDisconnected

and Msg =
    | HubReq of Req
    | HubEvt of Evt
    | InternalEvt of InternalEvt
with interface IMsg

let castEvt : CastEvt<Msg, Evt> =
    function
    | HubEvt evt -> Some evt
    | _ -> None

type Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, Evt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)
