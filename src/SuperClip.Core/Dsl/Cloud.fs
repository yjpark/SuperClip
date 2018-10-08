module SuperClip.Core.Dsl.Cloud

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.Remote.Meta
open Dap.Remote.Generator

open SuperClip.Core.Dsl.Types

let JoinReq =
    combo {
        var (M.custom (<@ Peer @>, "peer"))
        var (M.string "pass_hash")
    }

let JoinErr =
    union {
        kind "PassHashNotMatched"
        kind "InvalidChannelName"
        kind "JoinChannelFailed"
    }

let AuthErr =
    union {
        kind "InvalidToken"
    }

let SetItemErr =
    union {
        kind "InvalidSource"
        kind "InvalidChannel"
    }

let Evt =
    union {
        case "OnPeerJoin" (fields {
            var (M.custom (<@ Peer @>, "peer"))
        })
        case "OnPeerLeft" (fields {
            var (M.custom (<@ Peer @>, "peer"))
        })
        case "OnItemChanged" (fields {
            var (M.custom (<@ Item @>, "item"))
        })
    }
let Cloud =
    stub {
        req Do "Join"
        req Do "Auth"
        req Do "Leave"
        req Do "SetItem"
    }

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Cloud.fs"],
            G.QualifiedModule ("SuperClip.Core.Cloud",
                [
                    G.JsonRecord <@ JoinReq @>
                    |> G.AsDisplay """(this.Peer, (String.capped 12 this.PassHash))"""
                    ["type JoinRes = JsonString"]
                    G.JsonUnion <@ JoinErr @>
                    ["type AuthReq = JsonString"]
                    ["type AuthRes = Peers"]
                    G.JsonUnion <@ AuthErr @>
                    ["type LeaveReq = AuthReq"]
                    ["type LeaveRes = JsonBool"]
                    ["type LeaveErr = AuthErr"]
                    ["type SetItemReq = Item"]
                    ["type SetItemRes = JsonNil"]
                    G.JsonUnion <@ SetItemErr @>
                    G.JsonUnion <@ Evt @>
                    @ [
                        "    interface IEvent with"
                        "        member this.Kind = Union.getKind<Evt> this"
                    ]
                    G.Stub <@ Cloud @>
                ]
            )
        )]
