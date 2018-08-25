[<AutoOpen>]
module SuperClip.Core.Types

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Content =
    | Text of string
with
    member this.IsEmpty =
        match this with
        | Text text ->
            String.IsNullOrWhiteSpace text
    member this.Hash =
        if this.IsEmpty then
            ""
        else
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
    member this.Encrypt (cryptoKey : string) =
        match this with
        | Text text ->
            Des.encrypt cryptoKey text
            |> Text
    member this.Decrypt (runner : IRunner) (cryptoKey : string) =
        match this with
        | Text text ->
            Des.decrypt runner cryptoKey text
            |> Text

type Device = {
    Guid : Guid
    Name : string
} with
    static member Create guid name = {
        Guid = guid
        Name = name
    }
    static member New name =
        {
            Guid = (System.Guid.NewGuid()) .ToString ()
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

type ChannelKey = string

type Channel = {
    Guid : Guid
    Name : string
} with
    static member CalcGuid (name : string) =
        name
        |> fun n -> n.Trim ()
        |> calcSha256Sum2WithSalt "chiepuyiawaeR9aij6fiech7osh8kesh"
    static member Create guid name =
        {
            Guid = guid
            Name = name
        }
    static member CreateWithName name =
        {
            Guid = Channel.CalcGuid name
            Name = name
        }
    static member New name =
        {
            Guid = ""
            Name = name
        }
    static member JsonDecoder =
        D.decode Channel.Create
        |> D.required "guid" D.string
        |> D.required "name" D.string
    static member JsonEncoder (this : Channel) =
        E.object [
            "guid", E.string this.Guid
            "name", E.string this.Name
        ]
    interface IJson with
        member this.ToJson () =
            Channel.JsonEncoder this
    member this.Key : ChannelKey =
        if this.Guid <> "" then
            this.Guid
        elif this.Name <> "" then
            Channel.CalcGuid this.Name
        else
            "_N/A_"

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
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "device" Device.JsonDecoder
    static member JsonEncoder (this : Peer) =
        E.object [
            "channel", E.json this.Channel
            "device", E.json this.Device
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
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "devices" ^<| D.list Device.JsonDecoder
    static member JsonEncoder (this : Peers) =
        E.object [
            "channel", E.json this.Channel
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
    Source : Source
    Content : Content
} with
    static member Create time source content =
        {
            Time = time
            Source = source
            Content = content
        }
    static member Empty =
        Item.Create Instant.MinValue Local (Text "")
    member this.IsEmpty = this.Content.IsEmpty
    member this.Hash = this.Content.Hash
    static member JsonDecoder =
        D.decode Item.Create
        |> D.required "time" D.instant
        |> D.required "source" Source.JsonDecoder
        |> D.required "content" Content.JsonDecoder
    static member JsonEncoder (this : Item) =
        E.object [
            "time", E.instant this.Time
            "source", Source.JsonEncoder this.Source
            "content", Content.JsonEncoder this.Content
        ]
    interface IJson with
        member this.ToJson () =
            Item.JsonEncoder this
    member this.ForCloud (peer : Peer) (cryptoKey : string) =
        {this with
            Source = Cloud peer
            Content = this.Content.Encrypt cryptoKey
        }
    member this.Decrypt (runner : IRunner) (cryptoKey : string) =
        {this with
            Content = this.Content.Decrypt runner cryptoKey
        }

