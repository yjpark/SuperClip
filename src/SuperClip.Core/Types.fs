[<AutoOpen>]
module SuperClip.Core.Types

open System
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

type Content =
    | Text of string
with
    member this.Hash =
        match this with
        | Text text -> calcSha256Sum text
    static member JsonSpec : CaseSpec<Content> list =
        [
            CaseSpec<Content>.Create "Text" [
                FieldSpec.Create<String> String.JsonEncoder String.JsonDecoder
            ]
        ]
    static member JsonDecoder = D.union Content.JsonSpec
    static member JsonEncoder = E.union Content.JsonSpec
    interface IJson with
        member this.ToJson () =
            Content.JsonEncoder this

type Device = {
    Guid : string
    Name : string
} with
    static member Create guid name = {
        Guid = guid
        Name = name
    }
    static member JsonDecoder =
        D.decode Device.Create
        |> D.required "guid" D.string
        |> D.required "name" D.string
    static member JsonEncoder (this : Device) =
        E.object [
            "guid", E.string this.Guid
            "name", E.string this.Name
        ]
    interface IJson with
        member this.ToJson () =
            Device.JsonEncoder this

type Channel = string

type Peer = {
    Channel : Channel
    Device : Device
} with
    static member Create channel device = {
        Channel = channel
        Device = device
    }
    static member JsonDecoder =
        D.decode Peer.Create
        |> D.required "channel" D.string
        |> D.required "device" Device.JsonDecoder
    static member JsonEncoder (this : Peer) =
        E.object [
            "channel", E.string this.Channel
            "device", Device.JsonEncoder this.Device
        ]
    interface IJson with
        member this.ToJson () =
            Peer.JsonEncoder this

type Peers = {
    Channel : Channel
    Devices : Device list
} with
    static member Create channel devices = {
        Channel = channel
        Devices = devices
    }
    static member JsonDecoder =
        D.decode Peers.Create
        |> D.required "channel" D.string
        |> D.required "devices" ^<| D.list Device.JsonDecoder
    static member JsonEncoder (this : Peers) =
        E.object [
            "channel", E.string this.Channel
            "devices", E.list <| List.map Device.JsonEncoder this.Devices
        ]
    interface IJson with
        member this.ToJson () = Peers.JsonEncoder this

type Source =
    | Local
    | Cloud of Peer
with
    static member JsonSpec : CaseSpec<Source> list =
        [
            CaseSpec<Source>.Create "Local" []
            CaseSpec<Source>.Create "Cloud" [
                FieldSpec.Create<Peer> Peer.JsonEncoder Peer.JsonDecoder
            ]
        ]
    static member JsonDecoder = D.union Source.JsonSpec
    static member JsonEncoder = E.union Source.JsonSpec
    interface IJson with
        member this.ToJson () =
            Source.JsonEncoder this

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
    static member JsonDecoder =
        D.decode Item.Create
        |> D.required "time" D.instant
        |> D.required "content" Content.JsonDecoder
        |> D.required "source" Source.JsonDecoder
    static member JsonEncoder (this : Item) =
        E.object [
            "time", E.instant this.Time
            "content", Content.JsonEncoder this.Content
            "source", Source.JsonEncoder this.Source
        ]
    interface IJson with
        member this.ToJson () =
            Item.JsonEncoder this
