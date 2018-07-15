module SuperClip.Core.Primary.Agent

open Dap.Platform

open SuperClip.Core.Primary.Types
module Logic = SuperClip.Core.Primary.Logic

[<Literal>]
let Kind = "PrimaryClipboard"

type Args = SuperClip.Core.Primary.Types.Args
type Agent = IAgent<Req, Evt>

let registerAsync' kind args env =
    let spec = Logic.spec args
    env |> Env.registerAsync spec kind

let registerAsync a b = registerAsync' Kind a b
