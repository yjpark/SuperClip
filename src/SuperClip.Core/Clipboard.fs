[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Clipboard

open Dap.Platform
open NodaTime

type Content =
    | Text of string

type Source =
    | Local
    | Lan of name : string * address : string

type Item = {
    Time : Instant
    Content : Content
    Source : Source
} with
    static member Create time content source =
        {
            Time = time
            Content = content
            Source = source
        }
    static member Empty =
        Item.Create Instant.MinValue (Text "") Local

type Req =
    | DoGet of Callback<Item>
    | DoSet of Content * Callback<unit>
with interface IReq

type Evt =
    | OnChanged of Item
with interface IEvt

let DoSet' content callback =
    DoSet (content, callback)