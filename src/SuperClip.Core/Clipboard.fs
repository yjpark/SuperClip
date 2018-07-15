[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Clipboard

open Dap.Platform
open NodaTime

type Content =
    | Text of string

type Item = {
    Time : Instant
    Index : int
    Content : Content
} with
    static member Empty =
        {
            Time = Instant.MinValue
            Index = 0
            Content = Text ""
        }

type Req =
    | DoGet of Callback<Item>
    | DoSet of Content * Callback<unit>
with interface IReq

type Evt =
    | OnChanged of Item
with interface IEvt

let DoSet' content callback =
    DoSet (content, callback)