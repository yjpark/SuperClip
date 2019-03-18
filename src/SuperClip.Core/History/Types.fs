module SuperClip.Core.History.Types

open Dap.Prelude
open Dap.Context
open Dap.Platform
open SuperClip.Core

type Args = HistoryArgs

and Model = {
    PinnedItems : Item list
    RecentItems : Item list
} with
    static member JsonEncoder : JsonEncoder<Model> =
        fun (this : Model) ->
            E.object [
                "pinned_items", (E.list Item.JsonEncoder) this.PinnedItems
                "recent_items", (E.list Item.JsonEncoder) this.RecentItems
            ]
    static member JsonDecoder : JsonDecoder<Model> =
        D.object (fun get ->
            {
                PinnedItems = get.Optional.Field "pinned_items" (D.list Item.JsonDecoder)
                    |> Option.defaultValue []
                RecentItems = get.Optional.Field "recent_items" (D.list Item.JsonDecoder)
                    |> Option.defaultValue []
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Model> (Model.JsonEncoder, Model.JsonDecoder)
    interface IJson with
        member this.ToJson () = Model.JsonEncoder this

and Req =
    | DoMerge of Model * Callback<unit>
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