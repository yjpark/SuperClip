module SuperClip.Core.History.Agent

open Dap.Platform

open SuperClip.Core.History.Types
module Logic = SuperClip.Core.History.Logic

[<Literal>]
let Kind = "ClipboardHistory"

type Args = SuperClip.Core.History.Types.Args
type Agent = SuperClip.Core.History.Types.Agent

let registerAsync' kind args env =
    let spec = Logic.spec args
    env |> Env.registerAsync spec kind

let registerAsync a b = registerAsync' Kind a b
