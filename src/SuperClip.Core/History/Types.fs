module SuperClip.Core.History.Types

open Dap.Platform
open SuperClip.Core

type Args = HistoryArgs

and Model = {
    PinnedItems : Item list
    RecentItems : Item list
}

and Req =
    | DoPin of Item * Callback<unit>
    | DoUnpin of Item * Callback<unit>
    | DoAdd of Item * Callback<unit>
    | DoRemove of Item * Callback<unit>
    | DoClear of Callback<Item list>
with interface IReq

and Evt =
    | OnHistoryChanged
with interface IEvt

and Msg =
    | Req of Req
    | Evt of Evt
with interface IMsg

and Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, Evt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)

let castEvt (msg : Msg) =
    match msg with
    | Evt evt -> Some evt
    | _ -> None

let DoAdd item callback = DoAdd (item, callback)