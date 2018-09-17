module SuperClip.Core.Primary.Types

open Dap.Platform
open SuperClip.Core

type Req = Clipboard.Req
type Evt = Clipboard.Evt

type Args = PrimaryClipboardArgs

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

and Agent (pack, param) =
    inherit PackAgent<IServicesPack, Agent, Args, Model, Msg, Req, Evt> (pack, param)
    override this.Runner = this
    static member Spawn pack param = new Agent (pack, param)

let castEvt (msg : Msg) =
    match msg with
    | Evt evt -> Some evt
    | _ -> None