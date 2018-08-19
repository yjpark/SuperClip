module SuperClip.Core.Channel.Types

open Dap.Platform
open SuperClip.Core

type Args = Channel

and Model = {
    Current : Item
    Devices : Device list
}

and Req =
    | DoSet of Item * Callback<unit>
    | DoAddDevice of Device * Callback<bool>
    | DoRemoveDevice of Device * Callback<bool>
    | DoSetDevices of Device list * Callback<int>
with interface IReq

and Evt =
    | OnChanged of Item
    | OnDeviceAdded of Device
    | OnDeviceRemoved of Device
with interface IEvt

and Msg =
    | Req of Req
    | Evt of Evt
with interface IMsg

and Agent (param) =
    inherit BaseAgent<Agent, Args, Model, Msg, Req, Evt> (param)
    override this.Runner = this
    static member Spawn (param) = new Agent (param)
    member this.Channel = this.Actor.Args
    member this.Current = this.Actor.State.Current
    member this.Devices = this.Actor.State.Devices

let castEvt (msg : Msg) =
    match msg with
    | Evt evt -> Some evt
    | _ -> None

let DoSet item callback = DoSet (item, callback)
let DoAddDevice device callback = DoAddDevice (device, callback)
let DoRemoveDevice device callback = DoRemoveDevice (device, callback)
let DoSetDevices devices callback = DoSetDevices (devices, callback)
