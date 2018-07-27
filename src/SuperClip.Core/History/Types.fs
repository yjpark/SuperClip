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
    RecentItems : Clipboard.Item list
    AllItems : Map<string, Clipboard.Item>
}

and Req =
    | DoAdd of Clipboard.Item * Callback<bool>      // ~> isNew
    | DoRemoveOne of Clipboard.Content * Callback<Clipboard.Item option>
    | DoRemoveMany of (Clipboard.Item -> bool) * Callback<Clipboard.Item list>
with interface IReq

and Msg =
    | Req of Req
with interface IMsg

and Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, NoEvt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)

let DoAdd' item callback = DoAdd (item, callback)