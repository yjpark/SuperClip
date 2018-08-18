module SuperClip.Core.History.Types

open Dap.Platform
open SuperClip.Core

type Args = {
    MaxSize : int
    RecentSize : int
} with
    static member New () =
        {
            MaxSize = 400
            RecentSize = 20
        }

and Model = {
    RecentItems : Item list
    AllItems : Map<string, Item>
}

and Req =
    | DoAdd of Item * Callback<bool>      // ~> isNew
    | DoRemoveOne of Content * Callback<Item option>
    | DoRemoveMany of (Item -> bool) * Callback<Item list>
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