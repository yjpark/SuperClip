[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Clipboard

open NodaTime

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