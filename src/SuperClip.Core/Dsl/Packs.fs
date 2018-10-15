module SuperClip.Core.Dsl.Packs

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.Platform.Dsl.Packs

open SuperClip.Core.Dsl.Types

let ICorePack =
    pack [ <@ ITickingPack @> ] {
        add_pack <@ ITickingPack @> (M.primaryClipboardService ())
        add (M.historyService ("Local"))
        register (M.historySpawner ())
    }

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Packs.fs"],
            G.AutoOpenModule ("SuperClip.Core.Packs",
                [
                    G.PackOpens
                    G.PackInterface <@ ICorePack @>
                ]
            )
        )
    ]
