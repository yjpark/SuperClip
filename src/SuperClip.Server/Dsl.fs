module SuperClip.Server.Dsl

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.Platform.Dsl.Packs
open Dap.WebSocket.Meta
open Dap.Remote.Meta
open Dap.Local.Farango.Dsl

open SuperClip.Core.Dsl
open SuperClip.Server.Meta

let ICloudHubPack =
    pack [ <@ ITickingPack @> ] {
        register_pack <@ ITickingPack @> (M.packetConn (logTraffic = true))
        register_pack <@ IDbPack @> (M.cloudHub ())
        register (M.cloudHubGateway ())
    }

let App =
    live {
        has <@ IDbPack @>
        has <@ ICloudHubPack @>
    }

let opens =
    [
        "open Dap.Local.Farango"
        "open SuperClip.Core"
    ]

let compile segments =
    [
        (*
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Server.Types",
                [
                ]
            )
        )
        *)
        G.File (segments, ["_Gen" ; "Packs.fs"],
            G.AutoOpenModule ("SuperClip.Server.Packs",
                [
                    G.PackOpens
                    opens
                    G.PackInterface <@ ICloudHubPack @>
                ]
            )
        )
        G.File (segments, ["_Gen"; "App.fs"],
            G.AutoOpenModule ("SuperClip.Server.App",
                [
                    G.PackOpens
                    opens
                    G.App <@ App @>
                ]
            )
        )
    ]
