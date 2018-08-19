module SuperClip.Forms.Session.Service

open Dap.Platform

open SuperClip.Forms.Session.Types
module Logic = SuperClip.Forms.Session.Logic

[<Literal>]
let Kind = "FormsSession"

type Args = SuperClip.Forms.Session.Types.Args
type Service = SuperClip.Forms.Session.Types.Agent

let addAsync' kind key args =
    Env.addServiceAsync (Logic.spec args) kind key

let get' kind key env =
    env |> Env.getService kind key :?> Service

let addAsync key = addAsync' Kind key
let get key = get' Kind key