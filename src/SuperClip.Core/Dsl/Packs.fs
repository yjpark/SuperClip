module SuperClip.Core.Dsl.Packs

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.Platform.Dsl.Packs

open SuperClip.Core.Dsl.Types

let ICorePack =
    pack [ <@ ILocalPack @> ] {
        add_pack <@ ILocalPack @> (M.primaryClipboard ())
        add (M.history (key = "Local"))
        register (M.history ())
    }

let compile segments =
    [
        G.File (segments, ["_Gen1" ; "Packs.fs"],
            G.AutoOpenModule ("SuperClip.Core.Packs",
                [
                    G.PackOpens
                    G.PackInterface <@ ICorePack @>
                ]
            )
        )
    ]
