module SuperClip.Core.History.Logic

open Dap.Prelude
open Dap.Context
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.History.Types

module TickerTypes = Dap.Platform.Ticker.Types

type ActorOperate = Operate<Agent, Model, Msg>

let private withoutItem (item : Item) (items : Item list) =
    let hash = item.Hash
    items
    |> List.filter (fun item' -> item'.Hash <> hash)

let private withItem (item : Item) (items : Item list) =
    item :: (withoutItem item items)

let private addItem (item : Item) (items : Item list) =
    let hash = item.Hash
    let item' =
        items
        |> List.tryFind (fun item' -> item'.Hash = hash)
        |> Option.defaultValue item
    item' :: (withoutItem item items)

let private hasItem (item : Item) (items : Item list) =
    let hash = item.Hash
    items
    |> List.exists (fun item' -> item'.Hash = hash)

let private doAdd req ((item, callback) : Item * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        if item.IsEmpty then
            (model, cmd)
        elif hasItem item model.PinnedItems then
            replyAfter runner callback <| ack req ()
            (runner, model, cmd)
            |=|> addSubCmd Evt OnHistoryChanged
        else
            replyAfter runner callback <| ack req ()
            let recentItems =
                addItem item model.RecentItems
                |> List.truncate runner.Actor.Args.RecentSize
            (runner, model, cmd)
            |-|> updateModel (fun m -> {m with RecentItems = recentItems})
            |=|> addSubCmd Evt OnHistoryChanged

let private doMerge req ((data, callback) : Model * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        replyAfter runner callback <| ack req ()
        let pinnedItems =
            data.PinnedItems
            |> List.filter (fun item ->
                not (hasItem item model.PinnedItems)
            )
        let pinnedItems =
            model.PinnedItems @ pinnedItems
            |> List.truncate runner.Actor.Args.PinnedSize
        let recentItems =
            data.RecentItems
            |> List.filter (fun item ->
                not (hasItem item model.RecentItems)
            )
        let recentItems =
            model.RecentItems @ recentItems
            |> List.truncate runner.Actor.Args.RecentSize
        (runner, model, cmd)
        |-|> updateModel (fun m ->
            {m with PinnedItems = pinnedItems ; RecentItems = recentItems}
        )|=|> addSubCmd Evt OnHistoryChanged

let private doPin req ((item, callback) : Item * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        if item.IsEmpty then
            (model, cmd)
        else
            replyAfter runner callback <| ack req ()
            let pinnedItems =
                withItem item model.PinnedItems
                |> List.truncate runner.Actor.Args.PinnedSize
            let recentItems = withoutItem item model.RecentItems
            (runner, model, cmd)
            |-|> updateModel (fun m -> {m with PinnedItems = pinnedItems ; RecentItems = recentItems})
            |=|> addSubCmd Evt OnHistoryChanged

let private doUnpin req ((item, callback) : Item * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        if item.IsEmpty then
            (model, cmd)
        else
            replyAfter runner callback <| ack req ()
            let pinnedItems = withoutItem item model.PinnedItems
            let recentItems =
                withItem item model.RecentItems
                |> List.truncate runner.Actor.Args.RecentSize
            (runner, model, cmd)
            |-|> updateModel (fun m -> {m with PinnedItems = pinnedItems ; RecentItems = recentItems})
            |=|> addSubCmd Evt OnHistoryChanged

let private doRemove req ((item, callback) : Item * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        replyAfter runner callback <| ack req ()
        let recentItems = withoutItem item model.RecentItems
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with RecentItems = recentItems})
        |=|> addSubCmd Evt OnHistoryChanged

let private doClear req (callback : Callback<Item list>) : ActorOperate =
    fun runner (model, cmd) ->
        replyAfter runner callback <| ack req model.RecentItems
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with RecentItems = []})
        |=|> addSubCmd Evt OnHistoryChanged

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
        match msg with
        | Req req ->
            match req with
            | DoMerge (a, b) -> doMerge req (a, b)
            | DoPin (a, b) -> doPin req (a, b)
            | DoUnpin (a, b) -> doUnpin req (a, b)
            | DoAdd (a, b) -> doAdd req (a, b)
            | DoRemove (a, b) -> doRemove req (a, b)
            | DoClear a -> doClear req a
        | Evt evt ->
            noOperation
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    //TODO: Load from disk
    fun runner args ->
        ({
            RecentItems = []
            PinnedItems = []
        }, noCmd)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)

