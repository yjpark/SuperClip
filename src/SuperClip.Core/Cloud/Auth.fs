[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Auth

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req = JsonString

type Res = Peers

type Error =
    | InvalidToken
with
    static member JsonEncoder = E.kind<Error>
    static member JsonDecoder = D.kind<Error>
    interface IJson with
        member this.ToJson () = Error.JsonEncoder this