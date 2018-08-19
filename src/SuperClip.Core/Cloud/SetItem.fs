[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.SetItem

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req = Item

type Res = JsonNil

type Error =
    | InvalidSource
    | InvalidChannel
with
    static member JsonEncoder = E.kind<Error>
    static member JsonDecoder = D.kind<Error>
    interface IJson with
        member this.ToJson () = Error.JsonEncoder this