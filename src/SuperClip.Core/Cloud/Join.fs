[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Join

#if FABLE_COMPILER
module E = Thoth.Json.Encode
module D = Thoth.Json.Decode
#else
open Newtonsoft.Json
module E = Thoth.Json.Net.Encode
module D = Thoth.Json.Net.Decode
#endif

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

type Error =
    | PassHashNotMatched
with
    static member JsonEncoder = E.kind<Error>
    static member JsonDecoder = D.kind<Error>
    interface IJson with
        member this.ToJson () = Error.JsonEncoder this