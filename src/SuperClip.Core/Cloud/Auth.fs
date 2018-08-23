[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Auth

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req = JsonString

type Res = Peers

type Err =
    | InvalidToken
with
    static member JsonEncoder = E.kind<Err>
    static member JsonDecoder = D.kind<Err>
    interface IJson with
        member this.ToJson () = Err.JsonEncoder this