[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.SetItem

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req = Item

type Res = JsonNil

type Err =
    | InvalidSource
    | InvalidChannel
with
    static member JsonEncoder = E.kind<Err>
    static member JsonDecoder = D.kind<Err>
    interface IJson with
        member this.ToJson () = Err.JsonEncoder this