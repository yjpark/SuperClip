module SuperClip.Core.Channel.Service

open Dap.Platform

open SuperClip.Core.Channel.Types
module Logic = SuperClip.Core.Channel.Logic

[<Literal>]
let Kind = "ClipboardChannel"

type Args = SuperClip.Core.Channel.Types.Args
type Service = SuperClip.Core.Channel.Types.Agent

let addAsync' kind key (args : Args) env =
    env |> Env.addServiceAsync (Logic.spec args) kind key

let get' kind key env =
    env |> Env.getService kind key :?> Service

let tryFind' kind key env =
    env
    |> Env.tryFindService kind key
    |> Option.map (fun s -> s :?> Service)

let addAsync key = addAsync' Kind key
let get key = get' Kind key
let tryFind key = tryFind' Kind key

