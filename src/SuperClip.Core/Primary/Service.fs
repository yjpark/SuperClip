module SuperClip.Core.Primary.Service

open Dap.Platform

open SuperClip.Core.Primary.Types
module Logic = SuperClip.Core.Primary.Logic

[<Literal>]
let Kind = "PrimaryClipboard"

type Args = SuperClip.Core.Primary.Types.Args
type Service = IAgent<Req, Evt>

let addAsync' kind key args =
    Env.addServiceAsync (Logic.spec args) kind key

let get' kind key env =
    env |> Env.getService kind key :?> Service

let addAsync key = addAsync' Kind key
let get key = get' Kind key