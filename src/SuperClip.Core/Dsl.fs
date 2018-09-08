module SuperClip.Core.Dsl

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform

let Content =
    union {
        case "Text" (fields {
            var (M.string "content")
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
            G.Module ("SuperClip.Core.Types",
                [
                    [ "open Dap.Platform" ]
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
