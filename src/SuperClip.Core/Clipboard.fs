[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Clipboard

open Dap.Platform
open NodaTime

type Content =
    | Text of string
with
    member this.Hash =
        match this with
        | Text text -> calcSha256Sum text

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
    member this.Hash = this.Content.Hash

type Req =
    | DoGet of Callback<Item>
    | DoSet of Content * Callback<unit>
with interface IReq

type Evt =
    | OnChanged of Item
with interface IEvt

type Agent = IAgent<Req, Evt>

let DoSet' content callback =
    DoSet (content, callback)