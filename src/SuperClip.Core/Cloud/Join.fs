[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Join

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req = {
    Peer : Peer
    PassHash : string
} with
    static member Create peer passHash = {
        Peer = peer
        PassHash = passHash
    }
    static member JsonDecoder =
        D.decode Req.Create
        |> D.required "peer" Peer.JsonDecoder
        |> D.required "pass_hash" D.string
    static member JsonEncoder (this : Req) =
        E.object [
            "peer", Peer.JsonEncoder this.Peer
            "pass_hash", E.string this.PassHash
        ]
    interface IJson with
        member this.ToJson () = Req.JsonEncoder this

type Res = JsonString

type Err =
    | PassHashNotMatched
    | InvalidChannelName
    | JoinChannelFailed
with
    static member JsonEncoder = E.kind<Err>
    static member JsonDecoder = D.kind<Err>
    interface IJson with
        member this.ToJson () = Err.JsonEncoder this