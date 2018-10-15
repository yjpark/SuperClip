[<AutoOpen>]
module SuperClip.Core.Types

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

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
            CaseSpec<Content>.Create ("Text", [
                S.string
            ])
            CaseSpec<Content>.Create ("Asset", [
                S.string
            ])
        ]
    static member JsonEncoder = E.union Content.JsonSpec'
    static member JsonDecoder = D.union Content.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Content> (Content.JsonEncoder, Content.JsonDecoder)
    interface IJson with
        member this.ToJson () = Content.JsonEncoder this

(*
 * Generated: <Record>
 *     IsJson
 *)
type Device = {
    Guid : (* Device *) Guid
    Name : (* Device *) string
} with
    static member Create guid name
            : Device =
        {
            Guid = (* Device *) guid
            Name = (* Device *) name
        }
    static member Default () =
        Device.Create
            (System.Guid.NewGuid().ToString()) (* Device *) (* guid *)
            "" (* Device *) (* name *)
    static member SetGuid ((* Device *) guid : Guid) (this : Device) =
        {this with Guid = guid}
    static member SetName ((* Device *) name : string) (this : Device) =
        {this with Name = name}
    static member UpdateGuid ((* Device *) update : Guid -> Guid) (this : Device) =
        this |> Device.SetGuid (update this.Guid)
    static member UpdateName ((* Device *) update : string -> string) (this : Device) =
        this |> Device.SetName (update this.Name)
    static member JsonEncoder : JsonEncoder<Device> =
        fun (this : Device) ->
            E.object [
                "guid", E.string (* Device *) this.Guid
                "name", E.string (* Device *) this.Name
            ]
    static member JsonDecoder : JsonDecoder<Device> =
        D.object (fun get ->
            {
                Guid = get.Required.Field (* Device *) "guid" D.string
                Name = get.Required.Field (* Device *) "name" D.string
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Device> (Device.JsonEncoder, Device.JsonDecoder)
    interface IJson with
        member this.ToJson () = Device.JsonEncoder this
    interface IObj
    member this.WithGuid ((* Device *) guid : Guid) =
        this |> Device.SetGuid guid
    member this.WithName ((* Device *) name : string) =
        this |> Device.SetName name

(*
 * Generated: <Record>
 *     IsJson
 *)
type Channel = {
    Guid : (* Channel *) Guid
    Name : (* Channel *) string
} with
    static member Create guid name
            : Channel =
        {
            Guid = (* Channel *) guid
            Name = (* Channel *) name
        }
    static member Default () =
        Channel.Create
            (System.Guid.NewGuid().ToString()) (* Channel *) (* guid *)
            "" (* Channel *) (* name *)
    static member SetGuid ((* Channel *) guid : Guid) (this : Channel) =
        {this with Guid = guid}
    static member SetName ((* Channel *) name : string) (this : Channel) =
        {this with Name = name}
    static member UpdateGuid ((* Channel *) update : Guid -> Guid) (this : Channel) =
        this |> Channel.SetGuid (update this.Guid)
    static member UpdateName ((* Channel *) update : string -> string) (this : Channel) =
        this |> Channel.SetName (update this.Name)
    static member JsonEncoder : JsonEncoder<Channel> =
        fun (this : Channel) ->
            E.object [
                "guid", E.string (* Channel *) this.Guid
                "name", E.string (* Channel *) this.Name
            ]
    static member JsonDecoder : JsonDecoder<Channel> =
        D.object (fun get ->
            {
                Guid = get.Required.Field (* Channel *) "guid" D.string
                Name = get.Required.Field (* Channel *) "name" D.string
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Channel> (Channel.JsonEncoder, Channel.JsonDecoder)
    interface IJson with
        member this.ToJson () = Channel.JsonEncoder this
    interface IObj
    member this.WithGuid ((* Channel *) guid : Guid) =
        this |> Channel.SetGuid guid
    member this.WithName ((* Channel *) name : string) =
        this |> Channel.SetName name

(*
 * Generated: <Record>
 *     IsJson
 *)
type Peer = {
    Channel : (* Peer *) Channel
    Device : (* Peer *) Device
} with
    static member Create channel device
            : Peer =
        {
            Channel = (* Peer *) channel
            Device = (* Peer *) device
        }
    static member Default () =
        Peer.Create
            (Channel.Default ()) (* Peer *) (* channel *)
            (Device.Default ()) (* Peer *) (* device *)
    static member SetChannel ((* Peer *) channel : Channel) (this : Peer) =
        {this with Channel = channel}
    static member SetDevice ((* Peer *) device : Device) (this : Peer) =
        {this with Device = device}
    static member UpdateChannel ((* Peer *) update : Channel -> Channel) (this : Peer) =
        this |> Peer.SetChannel (update this.Channel)
    static member UpdateDevice ((* Peer *) update : Device -> Device) (this : Peer) =
        this |> Peer.SetDevice (update this.Device)
    static member JsonEncoder : JsonEncoder<Peer> =
        fun (this : Peer) ->
            E.object [
                "channel", Channel.JsonEncoder (* Peer *) this.Channel
                "device", Device.JsonEncoder (* Peer *) this.Device
            ]
    static member JsonDecoder : JsonDecoder<Peer> =
        D.object (fun get ->
            {
                Channel = get.Required.Field (* Peer *) "channel" Channel.JsonDecoder
                Device = get.Required.Field (* Peer *) "device" Device.JsonDecoder
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Peer> (Peer.JsonEncoder, Peer.JsonDecoder)
    interface IJson with
        member this.ToJson () = Peer.JsonEncoder this
    interface IObj
    member this.WithChannel ((* Peer *) channel : Channel) =
        this |> Peer.SetChannel channel
    member this.WithDevice ((* Peer *) device : Device) =
        this |> Peer.SetDevice device

(*
 * Generated: <Record>
 *     IsJson
 *)
type Peers = {
    Channel : (* Peers *) Channel
    Devices : (* Peers *) Device list
} with
    static member Create channel devices
            : Peers =
        {
            Channel = (* Peers *) channel
            Devices = (* Peers *) devices
        }
    static member Default () =
        Peers.Create
            (Channel.Default ()) (* Peers *) (* channel *)
            [] (* Peers *) (* devices *)
    static member SetChannel ((* Peers *) channel : Channel) (this : Peers) =
        {this with Channel = channel}
    static member SetDevices ((* Peers *) devices : Device list) (this : Peers) =
        {this with Devices = devices}
    static member UpdateChannel ((* Peers *) update : Channel -> Channel) (this : Peers) =
        this |> Peers.SetChannel (update this.Channel)
    static member UpdateDevices ((* Peers *) update : Device list -> Device list) (this : Peers) =
        this |> Peers.SetDevices (update this.Devices)
    static member JsonEncoder : JsonEncoder<Peers> =
        fun (this : Peers) ->
            E.object [
                "channel", Channel.JsonEncoder (* Peers *) this.Channel
                "devices", (E.list Device.JsonEncoder) (* Peers *) this.Devices
            ]
    static member JsonDecoder : JsonDecoder<Peers> =
        D.object (fun get ->
            {
                Channel = get.Required.Field (* Peers *) "channel" Channel.JsonDecoder
                Devices = get.Required.Field (* Peers *) "devices" (D.list Device.JsonDecoder)
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Peers> (Peers.JsonEncoder, Peers.JsonDecoder)
    interface IJson with
        member this.ToJson () = Peers.JsonEncoder this
    interface IObj
    member this.WithChannel ((* Peers *) channel : Channel) =
        this |> Peers.SetChannel channel
    member this.WithDevices ((* Peers *) devices : Device list) =
        this |> Peers.SetDevices devices

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
            CaseSpec<Source>.Create ("Local", [])
            CaseSpec<Source>.Create ("Cloud", [
                Peer.JsonSpec
            ])
        ]
    static member JsonEncoder = E.union Source.JsonSpec'
    static member JsonDecoder = D.union Source.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Source> (Source.JsonEncoder, Source.JsonDecoder)
    interface IJson with
        member this.ToJson () = Source.JsonEncoder this

(*
 * Generated: <Record>
 *     IsJson
 *)
type Item = {
    Time : (* Item *) Instant
    Source : Source
    Content : Content
} with
    static member Create time source content
            : Item =
        {
            Time = (* Item *) time
            Source = source
            Content = content
        }
    static member SetTime ((* Item *) time : Instant) (this : Item) =
        {this with Time = time}
    static member SetSource (source : Source) (this : Item) =
        {this with Source = source}
    static member SetContent (content : Content) (this : Item) =
        {this with Content = content}
    static member UpdateTime ((* Item *) update : Instant -> Instant) (this : Item) =
        this |> Item.SetTime (update this.Time)
    static member UpdateSource (update : Source -> Source) (this : Item) =
        this |> Item.SetSource (update this.Source)
    static member UpdateContent (update : Content -> Content) (this : Item) =
        this |> Item.SetContent (update this.Content)
    static member JsonEncoder : JsonEncoder<Item> =
        fun (this : Item) ->
            E.object [
                "time", E.instant (* Item *) this.Time
                "source", Source.JsonEncoder this.Source
                "content", Content.JsonEncoder this.Content
            ]
    static member JsonDecoder : JsonDecoder<Item> =
        D.object (fun get ->
            {
                Time = get.Required.Field (* Item *) "time" D.instant
                Source = get.Required.Field "source" Source.JsonDecoder
                Content = get.Required.Field "content" Content.JsonDecoder
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Item> (Item.JsonEncoder, Item.JsonDecoder)
    interface IJson with
        member this.ToJson () = Item.JsonEncoder this
    interface IObj
    member this.WithTime ((* Item *) time : Instant) =
        this |> Item.SetTime time
    member this.WithSource (source : Source) =
        this |> Item.SetSource source
    member this.WithContent (content : Content) =
        this |> Item.SetContent content

(*
 * Generated: <Record>
 *     IsJson
 *)
type PrimaryClipboardArgs = {
    CheckInterval : (* PrimaryClipboardArgs *) Duration
    TimeoutDuration : (* PrimaryClipboardArgs *) Duration
} with
    static member Create checkInterval timeoutDuration
            : PrimaryClipboardArgs =
        {
            CheckInterval = (* PrimaryClipboardArgs *) checkInterval
            TimeoutDuration = (* PrimaryClipboardArgs *) timeoutDuration
        }
    static member Default () =
        PrimaryClipboardArgs.Create
            (decodeJsonString Duration.JsonDecoder """0:00:00:00.5""") (* PrimaryClipboardArgs *) (* checkInterval *)
            (decodeJsonString Duration.JsonDecoder """0:00:00:01""") (* PrimaryClipboardArgs *) (* timeoutDuration *)
    static member SetCheckInterval ((* PrimaryClipboardArgs *) checkInterval : Duration) (this : PrimaryClipboardArgs) =
        {this with CheckInterval = checkInterval}
    static member SetTimeoutDuration ((* PrimaryClipboardArgs *) timeoutDuration : Duration) (this : PrimaryClipboardArgs) =
        {this with TimeoutDuration = timeoutDuration}
    static member UpdateCheckInterval ((* PrimaryClipboardArgs *) update : Duration -> Duration) (this : PrimaryClipboardArgs) =
        this |> PrimaryClipboardArgs.SetCheckInterval (update this.CheckInterval)
    static member UpdateTimeoutDuration ((* PrimaryClipboardArgs *) update : Duration -> Duration) (this : PrimaryClipboardArgs) =
        this |> PrimaryClipboardArgs.SetTimeoutDuration (update this.TimeoutDuration)
    static member JsonEncoder : JsonEncoder<PrimaryClipboardArgs> =
        fun (this : PrimaryClipboardArgs) ->
            E.object [
                "check_interval", DurationFormat.Second.JsonEncoder (* PrimaryClipboardArgs *) this.CheckInterval
                "timeout_duration", DurationFormat.Second.JsonEncoder (* PrimaryClipboardArgs *) this.TimeoutDuration
            ]
    static member JsonDecoder : JsonDecoder<PrimaryClipboardArgs> =
        D.object (fun get ->
            {
                CheckInterval = get.Required.Field (* PrimaryClipboardArgs *) "check_interval" DurationFormat.Second.JsonDecoder
                TimeoutDuration = get.Required.Field (* PrimaryClipboardArgs *) "timeout_duration" DurationFormat.Second.JsonDecoder
            }
        )
    static member JsonSpec =
        FieldSpec.Create<PrimaryClipboardArgs> (PrimaryClipboardArgs.JsonEncoder, PrimaryClipboardArgs.JsonDecoder)
    interface IJson with
        member this.ToJson () = PrimaryClipboardArgs.JsonEncoder this
    interface IObj
    member this.WithCheckInterval ((* PrimaryClipboardArgs *) checkInterval : Duration) =
        this |> PrimaryClipboardArgs.SetCheckInterval checkInterval
    member this.WithTimeoutDuration ((* PrimaryClipboardArgs *) timeoutDuration : Duration) =
        this |> PrimaryClipboardArgs.SetTimeoutDuration timeoutDuration

(*
 * Generated: <Record>
 *     IsJson
 *)
type HistoryArgs = {
    MaxSize : (* HistoryArgs *) int
    RecentSize : (* HistoryArgs *) int
} with
    static member Create maxSize recentSize
            : HistoryArgs =
        {
            MaxSize = (* HistoryArgs *) maxSize
            RecentSize = (* HistoryArgs *) recentSize
        }
    static member Default () =
        HistoryArgs.Create
            400 (* HistoryArgs *) (* maxSize *)
            20 (* HistoryArgs *) (* recentSize *)
    static member SetMaxSize ((* HistoryArgs *) maxSize : int) (this : HistoryArgs) =
        {this with MaxSize = maxSize}
    static member SetRecentSize ((* HistoryArgs *) recentSize : int) (this : HistoryArgs) =
        {this with RecentSize = recentSize}
    static member UpdateMaxSize ((* HistoryArgs *) update : int -> int) (this : HistoryArgs) =
        this |> HistoryArgs.SetMaxSize (update this.MaxSize)
    static member UpdateRecentSize ((* HistoryArgs *) update : int -> int) (this : HistoryArgs) =
        this |> HistoryArgs.SetRecentSize (update this.RecentSize)
    static member JsonEncoder : JsonEncoder<HistoryArgs> =
        fun (this : HistoryArgs) ->
            E.object [
                "max_size", E.int (* HistoryArgs *) this.MaxSize
                "recent_size", E.int (* HistoryArgs *) this.RecentSize
            ]
    static member JsonDecoder : JsonDecoder<HistoryArgs> =
        D.object (fun get ->
            {
                MaxSize = get.Required.Field (* HistoryArgs *) "max_size" D.int
                RecentSize = get.Required.Field (* HistoryArgs *) "recent_size" D.int
            }
        )
    static member JsonSpec =
        FieldSpec.Create<HistoryArgs> (HistoryArgs.JsonEncoder, HistoryArgs.JsonDecoder)
    interface IJson with
        member this.ToJson () = HistoryArgs.JsonEncoder this
    interface IObj
    member this.WithMaxSize ((* HistoryArgs *) maxSize : int) =
        this |> HistoryArgs.SetMaxSize maxSize
    member this.WithRecentSize ((* HistoryArgs *) recentSize : int) =
        this |> HistoryArgs.SetRecentSize recentSize