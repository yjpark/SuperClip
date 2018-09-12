module SuperClip.Forms.Dsl

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator

open SuperClip.Core.Dsl

let Credential =
    combo {
        var (M.custom (<@ Device @>, "device"))
        var (M.custom (<@ Channel @>, "channel"))
        var (M.string "pass_hash")
        var (M.string "crypto_key")
        var (M.string "token")
    }

let PrefProperties =
    combo {
        option (M.custom (<@ Credential @>, "credential"))
    }

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Forms.Types",
                [
                    [ "open SuperClip.Core" ]
                    G.JsonRecord <@ Credential @>
                    G.FinalClass (<@ PrefProperties @>, [])
                ]
            )
        )
    ]
