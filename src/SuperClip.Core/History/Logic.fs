module SuperClip.Core.History.Logic

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.History.Types

module TickerTypes = Dap.Platform.Ticker.Types
module TickerService = Dap.Platform.Ticker.Service

type ActorOperate = Operate<Agent, Model, Msg>

let private doAdd req ((item, callback) : Item * Callback<bool>) : ActorOperate =
    fun runner (model, cmd) ->
        let hash = item.Hash
        let recentItems =
            model.AllItems
            |> Map.tryFind hash
            |> function
                | Some item ->
                    replyAfter runner callback <| ack req false
                    model.RecentItems
                    |> List.filter (fun item -> item.Hash <> hash)
                | None ->
                    replyAfter runner callback <| ack req true
                    model.RecentItems
        let recentItems =
            item :: recentItems
            |> List.truncate runner.Actor.Args.RecentSize
        let allItems =
            model.AllItems
            |> Map.add hash item
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with RecentItems = recentItems ; AllItems = allItems})
        |=|> addSubCmd Evt OnHistoryChanged

let private fillRecentItems (runner : Agent) (allItems : Map<string, Item>) (recentItems : Item list) =
    if (recentItems |> List.length) < runner.Actor.Args.RecentSize then
        //TODO
        recentItems
    else
        recentItems

let private doRemoveOne req ((content, callback) : Content * Callback<Item option>) : ActorOperate =
    fun runner (model, cmd) ->
        let hash = content.Hash
        model.AllItems
        |> Map.tryFind hash
        |> function
            | Some item ->
                reply runner callback <| ack req ^<| Some item
                let allItems =
                    model.AllItems
                    |> Map.remove hash
                let recentItems =
                    model.RecentItems
                    |> List.filter (fun item -> item.Hash <> hash)
                    |> fillRecentItems runner allItems
                (runner, model, cmd)
                |-|> updateModel (fun m -> {m with RecentItems = recentItems ; AllItems = allItems})
                |=|> addSubCmd Evt OnHistoryChanged
            | None ->
                reply runner callback <| ack req None
                (model, cmd)

let private doRemoveMany req ((predicate, callback) : (Item -> bool) * Callback<Item list>) : ActorOperate =
    fun runner (model, cmd) ->
        let (removed, left) =
            model.AllItems
            |> Map.partition (fun _k v -> predicate v)
        if removed = Map.empty then
            (model, cmd)
        else
            let recentItems =
                model.RecentItems
                |> List.filter (fun item -> not (removed |> Map.containsKey item.Hash))
                |> fillRecentItems runner left
            reply runner callback <| ack req (removed |> Map.toList |> List.map snd)
            (runner, model, cmd)
            |-|> updateModel (fun m -> {m with RecentItems = recentItems ; AllItems = left})
            |=|> addSubCmd Evt OnHistoryChanged

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
        match msg with
        | Req req ->
            match req with
            | DoAdd (a, b) -> doAdd req (a, b)
            | DoRemoveOne (a, b) -> doRemoveOne req (a, b)
            | DoRemoveMany (a, b) -> doRemoveMany req (a, b)
        | Evt evt ->
            noOperation
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    fun runner args ->
        ({
            RecentItems = []
            AllItems = Map.empty
        }, noCmd)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)

