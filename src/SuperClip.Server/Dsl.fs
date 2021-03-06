module SuperClip.Server.Dsl

open Dap.Context.Meta
open Dap.Context.Meta.Net
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Meta.Net
open Dap.Platform.Generator
open Dap.Platform.Dsl.Packs
open Dap.WebSocket.Meta
open Dap.Remote.Meta
open Dap.Remote.Meta.Net
open Dap.Remote.Dashboard.Dsl.Pack
open Dap.Local.Farango.Dsl

open SuperClip.Core.Dsl
open SuperClip.Core.Dsl.Types
open SuperClip.Server.Meta

let IServerPack =
    pack [ <@ IDbPack @> ] {
        register (M.channel ())
    }

let ICloudHubPack =
    pack [ <@ IServerPack @> ] {
        //register_pack <@ ITickingPack @> (M.packetConn (logTraffic = true))
        register_pack <@ IServerPack @> (M.cloudHub ())
        register (M.cloudHubGateway ())
    }

let App =
    live {
        has <@ IDbPack @>
        has <@ IServerPack @>
        has <@ IDashboardPack @>
        has <@ ICloudHubPack @>
    }

let opens =
    [
        "open Dap.Local.Farango"
        "open Dap.Remote.Dashboard"
        "open SuperClip.Core"
    ]

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Server.Types",
                [
                    G.PackOpens
                    opens
                    G.PackInterface <@ IServerPack @>
                ]
            )
        )
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
