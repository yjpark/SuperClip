module SuperClip.Core.Dsl.Types

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator

let Content =
    union {
        kind "NoContent"
        case "Text" (fields {
            var (M.string "content")
        })
        case "Asset" (fields {
            var (M.string "url")
        })
    }

let Device =
    combo {
        var (M.guid "guid")
        var (M.string "name")
    }

let Channel =
    combo {
        var (M.guid "guid")
        var (M.string "name")
    }

let Peer =
    combo {
        var (M.custom (<@ Channel @>, "channel"))
        var (M.custom (<@ Device @>, "device"))
    }

let Peers =
    combo {
        var (M.custom (<@ Channel @>, "channel"))
        list (M.custom (<@ Device @>, "devices"))
    }

let Source =
    union {
        kind "NoSource"
        kind "Local"
        case "Cloud" (fields {
            var (M.custom (<@ Peer @>, "sender"))
        })
    }

let Item =
    combo {
        var (M.instant "time")
        var (M.union (<@ Source @>, "source", "NoSource", ""))
        var (M.union (<@ Content @>, "content", "NoContent", ""))
    }

let PrimaryClipboardArgs =
    combo {
        var (M.duration (DurationFormat.Second, "check_interval", Duration.FromSeconds 0.5))
        var (M.duration (DurationFormat.Second, "timeout_duration", Duration.FromSeconds 1.0))
    }

let HistoryArgs =
    combo {
        var (M.int ("max_size", 400))
        var (M.int ("recent_size", 20))
    }

type M with
    static member primaryClipboardService () =
        let alias = "PrimaryTypes", "SuperClip.Core.Primary.Types"
        let args = JsonArgs "PrimaryTypes.Args"
        let type' = "IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>"
        let spec = "SuperClip.Core.Primary.Logic.spec"
        M.service ([alias], args, type', spec, "Clipboard", "Primary")
    static member historySpawner () =
        let alias = "HistoryTypes", "SuperClip.Core.History.Types"
        let args = JsonArgs "HistoryTypes.Args"
        let type' = "HistoryTypes.Agent"
        let spec = "SuperClip.Core.History.Logic.spec"
        M.spawner ([alias], args, type', spec, "History")
    static member historyService (key : Key) =
        M.historySpawner ()
        |> fun s -> s.ToService key

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Core.Types",
                [
                    G.PackOpens
                    G.JsonUnion <@ Content @>
                    |> G.Default "Content.NoContent"
                    G.JsonRecord <@ Device @>
                    G.JsonRecord <@ Channel @>
                    G.JsonRecord <@ Peer @>
                    G.JsonRecord <@ Peers @>
                    G.JsonUnion <@ Source @>
                    |> G.Default "Source.NoSource"
                    G.JsonRecord <@ Item @>
                    G.JsonRecord <@ PrimaryClipboardArgs @>
                    G.JsonRecord <@ HistoryArgs @>
                ]
            )
        )
        G.File (segments, ["_Gen" ; "Builder.fs"],
            G.BuilderModule ("SuperClip.Core.Builder",
                [
                    G.PlatformOpens
                    G.ValueBuilder <@ PrimaryClipboardArgs @>
                    G.ValueBuilder <@ HistoryArgs @>
                ]
            )
        )
    ]
