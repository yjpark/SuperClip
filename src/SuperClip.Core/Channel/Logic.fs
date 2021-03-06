module SuperClip.Core.Channel.Logic

open NodaTime
open Elmish

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.Channel.Types

module TickerTypes = Dap.Platform.Ticker.Types

type ActorOperate = Operate<Agent, Model, Msg>

let private doInit req ((channel, callback) : Channel * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        match model.Channel with
        | None ->
            if runner.Ident.Key = channel.Key then
                replyAfter runner callback <| ack req ()
                (runner, model, cmd)
                |=|> updateModel (fun m -> {m with Channel = Some channel})
            else
                reply runner callback <| nak req "Invalid_Channel" (channel)
                (model, cmd)
        | Some channel' ->
            reply runner callback <| nak req "Already_Init" (channel', channel)
            (model, cmd)

let private doSet req ((item, callback) : Item * Callback<unit>) : ActorOperate =
    fun runner (model, cmd) ->
        replyAfter runner callback <| ack req ()
        (runner, model, cmd)
        |-|> updateModel (fun m -> {m with Current = item})
        |=|> addSubCmd Evt ^<| OnChanged item

let private doAddDevice req ((device, callback) : Device * Callback<bool>) : ActorOperate =
    fun runner (model, cmd) ->
        model.Devices
        |> List.exists (fun d -> d.Guid = device.Guid)
        |> function
            | true ->
                reply runner callback <| ack req false
                (model, cmd)
            | false ->
                replyAfter runner callback <| ack req true
                (runner, model, cmd)
                |-|> updateModel (fun m -> {m with Devices = device :: m.Devices})
                |=|> addSubCmd Evt ^<| OnDeviceAdded device

let private doRemoveDevice req ((device, callback) : Device * Callback<bool>) : ActorOperate =
    fun runner (model, cmd) ->
        model.Devices
        |> List.filter (fun d -> d.Guid <> device.Guid)
        |> fun devices ->
            if devices.Length = model.Devices.Length then
                reply runner callback <| ack req false
                (model, cmd)
            else
                replyAfter runner callback <| ack req true
                (runner, model, cmd)
                |-|> updateModel (fun m -> {m with Devices = devices})
                |=|> addSubCmd Evt ^<| OnDeviceRemoved device

let private doSetDevices req ((devices, callback) : Device list * Callback<int>) : ActorOperate =
    fun runner (model, cmd) ->
        let removes =
            model.Devices
            |> List.map (fun device ->
                addSubCmd Evt ^<| OnDeviceRemoved device
            )
        let adds =
            devices
            |> List.map (fun device ->
                addSubCmd Evt ^<| OnDeviceAdded device
            )
        let update = updateModel (fun (m : Model) -> {m with Devices = devices})
        replyAfter runner callback <| ack req model.Devices.Length
        (removes @ [ update ] @ adds)
        |> List.reduce (|-|-)
        <| runner <| (model, cmd)

let private update : Update<Agent, Model, Msg> =
    fun runner msg model ->
        match msg with
        | Req req ->
            match req with
            | DoInit (a, b) -> doInit req (a, b)
            | DoSet (a, b) -> doSet req (a, b)
            | DoAddDevice (a, b) -> doAddDevice req (a, b)
            | DoRemoveDevice (a, b) -> doRemoveDevice req (a, b)
            | DoSetDevices (a, b) -> doSetDevices req (a, b)
        | Evt _evt -> noOperation
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    fun runner args ->
        ({
            Channel = None
            Current = Item.Empty
            Devices = []
        }, noCmd)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, Req, castEvt, init, update)

