[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Clipboard

open NodaTime
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

type Req =
    | DoGet of Callback<Item>
    | DoSet of Content * Callback<unit>
with interface IReq

type Evt =
    | OnSet of Item
    | OnChanged of Item
with interface IEvt

type Agent = IAgent<Req, Evt>

let DoSet content callback =
    DoSet (content, callback)