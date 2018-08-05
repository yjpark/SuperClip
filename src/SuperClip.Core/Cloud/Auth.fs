[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Auth

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

type Req = JsonString

type Res = Peers

type Error =
    | InvalidToken
with
    static member JsonEncoder = E.kind<Error>
    static member JsonDecoder = D.kind<Error>
    interface IJson with
        member this.ToJson () = Error.JsonEncoder this