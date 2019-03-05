module SuperClip.Core.Dsl.Types

open Dap.Context.Meta
open Dap.Context.Meta.Net
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Meta.Net
open Dap.Platform.Dsl.Packs
open Dap.Platform.Generator
open Dap.Context.CustomProperty

let Content =
    union {
        kind "NoContent"
        case "Text" (fields {
            var (M.string "content")
        })
        case "Asset" (fields {
            var (M.string "url")
        })
    }|> UnionMeta.SetInitValue (Some "NoContent")

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
    }|> UnionMeta.SetInitValue (Some "NoSource")

let Item =
    combo {
        var (M.instant "time")
        var (M.custom (<@ Source @>, "source"))
        var (M.custom (<@ Content @>, "content"))
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

let LocalClipboardProps =
    combo {
        var (M.bool ("support_on_changed"))
    }

let LocalClipboard =
    context <@ LocalClipboardProps @> {
        kind "LocalClipboard"
        channel (M.custom (<@ Content @>, "on_changed"))
        async_handler (M.unit "get") (M.custom (<@ Content @>, response))
        async_handler (M.custom (<@ Content @>, "set")) (M.unit response)
    }

type M with
    static member localClipboard (?name : string, ?spawner : string, ?kind : Kind, ?key : Key, ?aliases : ModuleAlias list) =
        let name = defaultArg name "ILocalClipboard"
        M.feature (name, ?spawner = spawner, ?kind = kind, ?key = key, ?aliases = aliases)
    static member primaryClipboard () =
        let alias = "PrimaryTypes", "SuperClip.Core.Primary.Types"
        let args = JsonArgs "PrimaryTypes.Args"
        let type' = "PrimaryTypes.Agent"
        let spec = "SuperClip.Core.Primary.Logic.spec"
        M.agent (args, type', spec, kind = "Clipboard", key = "Primary", aliases = [alias])
    static member history (?key : Key) =
        let alias = "HistoryTypes", "SuperClip.Core.History.Types"
        let args = JsonArgs "HistoryTypes.Args"
        let type' = "HistoryTypes.Agent"
        let spec = "SuperClip.Core.History.Logic.spec"
        M.agent (args, type', spec, kind = "History", ?key = key, aliases = [alias])

let ILocalPack =
    pack [ <@ ITickingPack @> ] {
        add (M.localClipboard ())
    }

type G with
    static member CorePack (feature : string option) =
        let feature = defaultArg feature "SuperClip.Core.Feature"
        [
            sprintf "type LocalClipboard = %s.LocalClipboard.Context" feature
        ]

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Core.Types",
                [
                    G.PackOpens
                    G.JsonUnion <@ Content @>
                    G.JsonRecord <@ Device @>
                    G.JsonRecord <@ Channel @>
                    G.JsonRecord <@ Peer @>
                    G.JsonRecord <@ Peers @>
                    G.JsonUnion <@ Source @>
                    G.JsonRecord <@ Item @>
                    G.JsonRecord <@ PrimaryClipboardArgs @>
                    G.JsonRecord <@ HistoryArgs @>
                    G.Combo <@ LocalClipboardProps @>
                    G.Feature <@ LocalClipboard @>
                    G.PackInterface <@ ILocalPack @>
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
