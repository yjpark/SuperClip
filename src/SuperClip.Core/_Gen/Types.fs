[<AutoOpen>]
module SuperClip.Core.Types

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

module TickerTypes = Dap.Platform.Ticker.Types

(*
 * Generated: <Union>
 *     IsJson
 *)
type Content =
    | Text of content : string
    | Asset of url : string
with
    static member CreateText content : Content =
        Text (content)
    static member CreateAsset url : Content =
        Asset (url)
    static member JsonSpec' : CaseSpec<Content> list =
        [
            CaseSpec<Content>.Create "Text" [
                S.string
            ]
            CaseSpec<Content>.Create "Asset" [
                S.string
            ]
        ]
    static member JsonEncoder = E.union Content.JsonSpec'
    static member JsonDecoder = D.union Content.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Content>
            Content.JsonEncoder Content.JsonDecoder
    interface IJson with
        member this.ToJson () = Content.JsonEncoder this

(*
 * Generated: <Record>
 *     IsJson
 *)
type Device = {
    Guid : Guid
    Name : string
} with
    static member Create guid name
            : Device =
        {
            Guid = guid
            Name = name
        }
    static member Default () =
        Device.Create
            (System.Guid.NewGuid().ToString())
            ""
    static member JsonEncoder : JsonEncoder<Device> =
        fun (this : Device) ->
            E.object [
                "guid", E.string this.Guid
                "name", E.string this.Name
            ]
    static member JsonDecoder : JsonDecoder<Device> =
        D.decode Device.Create
        |> D.required "guid" D.string
        |> D.required "name" D.string
    static member JsonSpec =
        FieldSpec.Create<Device>
            Device.JsonEncoder Device.JsonDecoder
    interface IJson with
        member this.ToJson () = Device.JsonEncoder this
    interface IObj
    member this.WithGuid (guid : Guid) = {this with Guid = guid}
    member this.WithName (name : string) = {this with Name = name}

(*
 * Generated: <Record>
 *     IsJson
 *)
type Channel = {
    Guid : Guid
    Name : string
} with
    static member Create guid name
            : Channel =
        {
            Guid = guid
            Name = name
        }
    static member Default () =
        Channel.Create
            (System.Guid.NewGuid().ToString())
            ""
    static member JsonEncoder : JsonEncoder<Channel> =
        fun (this : Channel) ->
            E.object [
                "guid", E.string this.Guid
                "name", E.string this.Name
            ]
    static member JsonDecoder : JsonDecoder<Channel> =
        D.decode Channel.Create
        |> D.required "guid" D.string
        |> D.required "name" D.string
    static member JsonSpec =
        FieldSpec.Create<Channel>
            Channel.JsonEncoder Channel.JsonDecoder
    interface IJson with
        member this.ToJson () = Channel.JsonEncoder this
    interface IObj
    member this.WithGuid (guid : Guid) = {this with Guid = guid}
    member this.WithName (name : string) = {this with Name = name}

(*
 * Generated: <Record>
 *     IsJson
 *)
type Peer = {
    Channel : Channel
    Device : Device
} with
    static member Create channel device
            : Peer =
        {
            Channel = channel
            Device = device
        }
    static member JsonEncoder : JsonEncoder<Peer> =
        fun (this : Peer) ->
            E.object [
                "channel", Channel.JsonEncoder this.Channel
                "device", Device.JsonEncoder this.Device
            ]
    static member JsonDecoder : JsonDecoder<Peer> =
        D.decode Peer.Create
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "device" Device.JsonDecoder
    static member JsonSpec =
        FieldSpec.Create<Peer>
            Peer.JsonEncoder Peer.JsonDecoder
    interface IJson with
        member this.ToJson () = Peer.JsonEncoder this
    interface IObj
    member this.WithChannel (channel : Channel) = {this with Channel = channel}
    member this.WithDevice (device : Device) = {this with Device = device}

(*
 * Generated: <Record>
 *     IsJson
 *)
type Peers = {
    Channel : Channel
    Devices : Device list
} with
    static member Create channel devices
            : Peers =
        {
            Channel = channel
            Devices = devices
        }
    static member JsonEncoder : JsonEncoder<Peers> =
        fun (this : Peers) ->
            E.object [
                "channel", Channel.JsonEncoder this.Channel
                "devices", (E.list Device.JsonEncoder) this.Devices
            ]
    static member JsonDecoder : JsonDecoder<Peers> =
        D.decode Peers.Create
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "devices" (D.list Device.JsonDecoder)
    static member JsonSpec =
        FieldSpec.Create<Peers>
            Peers.JsonEncoder Peers.JsonDecoder
    interface IJson with
        member this.ToJson () = Peers.JsonEncoder this
    interface IObj
    member this.WithChannel (channel : Channel) = {this with Channel = channel}
    member this.WithDevices (devices : Device list) = {this with Devices = devices}

(*
 * Generated: <Union>
 *     IsJson
 *)
type Source =
    | Local
    | Cloud of sender : Peer
with
    static member CreateLocal () : Source =
        Local
    static member CreateCloud sender : Source =
        Cloud (sender)
    static member JsonSpec' : CaseSpec<Source> list =
        [
            CaseSpec<Source>.Create "Local" []
            CaseSpec<Source>.Create "Cloud" [
                Peer.JsonSpec
            ]
        ]
    static member JsonEncoder = E.union Source.JsonSpec'
    static member JsonDecoder = D.union Source.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Source>
            Source.JsonEncoder Source.JsonDecoder
    interface IJson with
        member this.ToJson () = Source.JsonEncoder this

(*
 * Generated: <Record>
 *     IsJson
 *)
type Item = {
    Time : Instant
    Source : Source
    Content : Content
} with
    static member Create time source content
            : Item =
        {
            Time = time
            Source = source
            Content = content
        }
    static member JsonEncoder : JsonEncoder<Item> =
        fun (this : Item) ->
            E.object [
                "time", E.instant this.Time
                "source", Source.JsonEncoder this.Source
                "content", Content.JsonEncoder this.Content
            ]
    static member JsonDecoder : JsonDecoder<Item> =
        D.decode Item.Create
        |> D.required "time" D.instant
        |> D.required "source" Source.JsonDecoder
        |> D.required "content" Content.JsonDecoder
    static member JsonSpec =
        FieldSpec.Create<Item>
            Item.JsonEncoder Item.JsonDecoder
    interface IJson with
        member this.ToJson () = Item.JsonEncoder this
    interface IObj
    member this.WithTime (time : Instant) = {this with Time = time}
    member this.WithSource (source : Source) = {this with Source = source}
    member this.WithContent (content : Content) = {this with Content = content}

(*
 * Generated: <Record>
 *     IsJson
 *)
type PrimaryClipboardArgs = {
    CheckInterval : Duration
    TimeoutDuration : Duration
} with
    static member Create checkInterval timeoutDuration
            : PrimaryClipboardArgs =
        {
            CheckInterval = checkInterval
            TimeoutDuration = timeoutDuration
        }
    static member Default () =
        PrimaryClipboardArgs.Create
            (decodeJsonString Duration.JsonDecoder """0:00:00:00.5""")
            (decodeJsonString Duration.JsonDecoder """0:00:00:01""")
    static member JsonEncoder : JsonEncoder<PrimaryClipboardArgs> =
        fun (this : PrimaryClipboardArgs) ->
            E.object [
                "check_interval", DurationFormat.Second.JsonEncoder this.CheckInterval
                "timeout_duration", DurationFormat.Second.JsonEncoder this.TimeoutDuration
            ]
    static member JsonDecoder : JsonDecoder<PrimaryClipboardArgs> =
        D.decode PrimaryClipboardArgs.Create
        |> D.required "check_interval" DurationFormat.Second.JsonDecoder
        |> D.required "timeout_duration" DurationFormat.Second.JsonDecoder
    static member JsonSpec =
        FieldSpec.Create<PrimaryClipboardArgs>
            PrimaryClipboardArgs.JsonEncoder PrimaryClipboardArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = PrimaryClipboardArgs.JsonEncoder this
    interface IObj
    member this.WithCheckInterval (checkInterval : Duration) = {this with CheckInterval = checkInterval}
    member this.WithTimeoutDuration (timeoutDuration : Duration) = {this with TimeoutDuration = timeoutDuration}

(*
 * Generated: <Record>
 *     IsJson
 *)
type HistoryArgs = {
    MaxSize : int
    RecentSize : int
} with
    static member Create maxSize recentSize
            : HistoryArgs =
        {
            MaxSize = maxSize
            RecentSize = recentSize
        }
    static member Default () =
        HistoryArgs.Create
            400
            20
    static member JsonEncoder : JsonEncoder<HistoryArgs> =
        fun (this : HistoryArgs) ->
            E.object [
                "max_size", E.int this.MaxSize
                "recent_size", E.int this.RecentSize
            ]
    static member JsonDecoder : JsonDecoder<HistoryArgs> =
        D.decode HistoryArgs.Create
        |> D.required "max_size" D.int
        |> D.required "recent_size" D.int
    static member JsonSpec =
        FieldSpec.Create<HistoryArgs>
            HistoryArgs.JsonEncoder HistoryArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = HistoryArgs.JsonEncoder this
    interface IObj
    member this.WithMaxSize (maxSize : int) = {this with MaxSize = maxSize}
    member this.WithRecentSize (recentSize : int) = {this with RecentSize = recentSize}

type IServicesPackArgs =
    abstract Ticker : TickerTypes.Args with get

type IServicesPack =
    inherit ILogger
    abstract Env : IEnv with get
    abstract Args : IServicesPackArgs with get
    abstract Ticker : TickerTypes.Agent with get