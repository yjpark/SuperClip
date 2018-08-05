module SuperClip.Core.Primary.Types

open Dap.Platform
open SuperClip.Core

type Req = Clipboard.Req
type Evt = Clipboard.Evt

type Args = {
    CheckInterval : float<second> option
    TimeoutDuration : Duration
} with
    static member New () =
        {
            CheckInterval = Some 0.5<second>
            TimeoutDuration = Duration.FromSeconds 1.0
        }

and Model = {
    Current : Item
    Getting : bool
    GettingIndex : int
    NextGetTime : Instant
    TimeoutTime : Instant
    WaitingCallbacks : (IReq * Callback<Item>) list
}

and InternalEvt =
    | DoInit
    | OnTick of Instant * Duration
    | OnGet of Result<Content, exn>

and Msg =
    | Req of Req
    | Evt of Evt
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