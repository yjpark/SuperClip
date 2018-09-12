module SuperClip.Core.Dsl

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator

let Content =
    union {
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
        kind "Local"
        case "Cloud" (fields {
            var (M.custom (<@ Peer @>, "sender"))
        })
    }

let Item =
    combo {
        var (M.instant "time")
        var (M.union (<@ Source @>, "source"))
        var (M.union (<@ Content @>, "content"))
    }

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Core.Types",
                [
                    G.PlatformOpens
                    G.JsonUnion <@ Content @>
                    G.JsonRecord <@ Device @>
                    G.JsonRecord <@ Channel @>
                    G.JsonRecord <@ Peer @>
                    G.JsonRecord <@ Peers @>
                    G.JsonUnion <@ Source @>
                    G.JsonRecord <@ Item @>
                ]
            )
        )
    ]
