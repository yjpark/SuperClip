[<AutoOpen>]
module SuperClip.Core.Types

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

module Context = Dap.Platform.Context

(*
 * Generated: <Union>
 *     IsJson
 *)
type Content =
    | NoContent
    | Text of content : string
    | Asset of url : string
with
    static member CreateNoContent () : Content =
        NoContent
    static member CreateText content : Content =
        Text (content)
    static member CreateAsset url : Content =
        Asset (url)
    static member JsonSpec' : CaseSpec<Content> list =
        [
            CaseSpec<Content>.Create ("NoContent", [])
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
    static member Create
        (
            ?guid : (* Device *) Guid,
            ?name : (* Device *) string
        ) : Device =
        {
            Guid = (* Device *) guid
                |> Option.defaultWith (fun () -> (newGuid ()))
            Name = (* Device *) name
                |> Option.defaultWith (fun () -> "")
        }
    static member Default () = Device.Create ()
    static member SetGuid ((* Device *) guid : Guid) (this : Device) =
        {this with Guid = guid}
    static member SetName ((* Device *) name : string) (this : Device) =
        {this with Name = name}
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
    static member Create
        (
            ?guid : (* Channel *) Guid,
            ?name : (* Channel *) string
        ) : Channel =
        {
            Guid = (* Channel *) guid
                |> Option.defaultWith (fun () -> (newGuid ()))
            Name = (* Channel *) name
                |> Option.defaultWith (fun () -> "")
        }
    static member Default () = Channel.Create ()
    static member SetGuid ((* Channel *) guid : Guid) (this : Channel) =
        {this with Guid = guid}
    static member SetName ((* Channel *) name : string) (this : Channel) =
        {this with Name = name}
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
    static member Create
        (
            ?channel : (* Peer *) Channel,
            ?device : (* Peer *) Device
        ) : Peer =
        {
            Channel = (* Peer *) channel
                |> Option.defaultWith (fun () -> (Channel.Default ()))
            Device = (* Peer *) device
                |> Option.defaultWith (fun () -> (Device.Default ()))
        }
    static member Default () = Peer.Create ()
    static member SetChannel ((* Peer *) channel : Channel) (this : Peer) =
        {this with Channel = channel}
    static member SetDevice ((* Peer *) device : Device) (this : Peer) =
        {this with Device = device}
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
    static member Create
        (
            ?channel : (* Peers *) Channel,
            ?devices : (* Peers *) Device list
        ) : Peers =
        {
            Channel = (* Peers *) channel
                |> Option.defaultWith (fun () -> (Channel.Default ()))
            Devices = (* Peers *) devices
                |> Option.defaultWith (fun () -> [])
        }
    static member Default () = Peers.Create ()
    static member SetChannel ((* Peers *) channel : Channel) (this : Peers) =
        {this with Channel = channel}
    static member SetDevices ((* Peers *) devices : Device list) (this : Peers) =
        {this with Devices = devices}
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
    | NoSource
    | Local
    | Cloud of sender : Peer
with
    static member CreateNoSource () : Source =
        NoSource
    static member CreateLocal () : Source =
        Local
    static member CreateCloud sender : Source =
        Cloud (sender)
    static member JsonSpec' : CaseSpec<Source> list =
        [
            CaseSpec<Source>.Create ("NoSource", [])
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
    Source : (* Item *) Source
    Content : (* Item *) Content
} with
    static member Create
        (
            ?time : (* Item *) Instant,
            ?source : (* Item *) Source,
            ?content : (* Item *) Content
        ) : Item =
        {
            Time = (* Item *) time
                |> Option.defaultWith (fun () -> (getNow' ()))
            Source = (* Item *) source
                |> Option.defaultWith (fun () -> NoSource)
            Content = (* Item *) content
                |> Option.defaultWith (fun () -> NoContent)
        }
    static member Default () = Item.Create ()
    static member SetTime ((* Item *) time : Instant) (this : Item) =
        {this with Time = time}
    static member SetSource ((* Item *) source : Source) (this : Item) =
        {this with Source = source}
    static member SetContent ((* Item *) content : Content) (this : Item) =
        {this with Content = content}
    static member JsonEncoder : JsonEncoder<Item> =
        fun (this : Item) ->
            E.object [
                "time", E.instant (* Item *) this.Time
                "source", Source.JsonEncoder (* Item *) this.Source
                "content", Content.JsonEncoder (* Item *) this.Content
            ]
    static member JsonDecoder : JsonDecoder<Item> =
        D.object (fun get ->
            {
                Time = get.Required.Field (* Item *) "time" D.instant
                Source = get.Required.Field (* Item *) "source" Source.JsonDecoder
                Content = get.Required.Field (* Item *) "content" Content.JsonDecoder
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Item> (Item.JsonEncoder, Item.JsonDecoder)
    interface IJson with
        member this.ToJson () = Item.JsonEncoder this
    interface IObj
    member this.WithTime ((* Item *) time : Instant) =
        this |> Item.SetTime time
    member this.WithSource ((* Item *) source : Source) =
        this |> Item.SetSource source
    member this.WithContent ((* Item *) content : Content) =
        this |> Item.SetContent content

(*
 * Generated: <Record>
 *     IsJson
 *)
type PrimaryClipboardArgs = {
    CheckInterval : (* PrimaryClipboardArgs *) Duration
    TimeoutDuration : (* PrimaryClipboardArgs *) Duration
} with
    static member Create
        (
            ?checkInterval : (* PrimaryClipboardArgs *) Duration,
            ?timeoutDuration : (* PrimaryClipboardArgs *) Duration
        ) : PrimaryClipboardArgs =
        {
            CheckInterval = (* PrimaryClipboardArgs *) checkInterval
                |> Option.defaultWith (fun () -> (decodeJsonString DurationFormat.Second.JsonDecoder """0.5"""))
            TimeoutDuration = (* PrimaryClipboardArgs *) timeoutDuration
                |> Option.defaultWith (fun () -> (decodeJsonString DurationFormat.Second.JsonDecoder """1"""))
        }
    static member Default () = PrimaryClipboardArgs.Create ()
    static member SetCheckInterval ((* PrimaryClipboardArgs *) checkInterval : Duration) (this : PrimaryClipboardArgs) =
        {this with CheckInterval = checkInterval}
    static member SetTimeoutDuration ((* PrimaryClipboardArgs *) timeoutDuration : Duration) (this : PrimaryClipboardArgs) =
        {this with TimeoutDuration = timeoutDuration}
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
    static member Create
        (
            ?maxSize : (* HistoryArgs *) int,
            ?recentSize : (* HistoryArgs *) int
        ) : HistoryArgs =
        {
            MaxSize = (* HistoryArgs *) maxSize
                |> Option.defaultWith (fun () -> 400)
            RecentSize = (* HistoryArgs *) recentSize
                |> Option.defaultWith (fun () -> 20)
        }
    static member Default () = HistoryArgs.Create ()
    static member SetMaxSize ((* HistoryArgs *) maxSize : int) (this : HistoryArgs) =
        {this with MaxSize = maxSize}
    static member SetRecentSize ((* HistoryArgs *) recentSize : int) (this : HistoryArgs) =
        {this with RecentSize = recentSize}
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

(*
 * Generated: <Combo>
 *)
type LocalClipboardProps (owner : IOwner, key : Key) =
    inherit WrapProperties<LocalClipboardProps, IComboProperty> ()
    let target' = Properties.combo (owner, key)
    let supportOnChanged = target'.AddVar<(* LocalClipboardProps *) bool> (E.bool, D.bool, "support_on_changed", false, None)
    do (
        base.Setup (target')
    )
    static member Create (o, k) = new LocalClipboardProps (o, k)
    static member Default () = LocalClipboardProps.Create (noOwner, NoKey)
    static member AddToCombo key (combo : IComboProperty) =
        combo.AddCustom<LocalClipboardProps> (LocalClipboardProps.Create, key)
    override this.Self = this
    override __.Spawn (o, k) = LocalClipboardProps.Create (o, k)
    override __.SyncTo t = target'.SyncTo t.Target
    member __.SupportOnChanged (* LocalClipboardProps *) : IVarProperty<bool> = supportOnChanged

(*
 * Generated: <Context>
 *)
type ILocalClipboard =
    inherit IContext<LocalClipboardProps>
    abstract LocalClipboardProps : LocalClipboardProps with get
    abstract OnChanged : IChannel<Content> with get
    abstract GetAsync : IAsyncHandler<unit, Content> with get
    abstract SetAsync : IAsyncHandler<Content, unit> with get

(*
 * Generated: <Context>
 *)
[<Literal>]
let LocalClipboardKind = "LocalClipboard"

[<AbstractClass>]
type BaseLocalClipboard<'context when 'context :> ILocalClipboard> (logging : ILogging) =
    inherit CustomContext<'context, ContextSpec<LocalClipboardProps>, LocalClipboardProps> (logging, new ContextSpec<LocalClipboardProps>(LocalClipboardKind, LocalClipboardProps.Create))
    let onChanged = base.Channels.Add<Content> (Content.JsonEncoder, Content.JsonDecoder, "on_changed")
    let getAsync = base.AsyncHandlers.Add<unit, Content> (E.unit, D.unit, Content.JsonEncoder, Content.JsonDecoder, "get")
    let setAsync = base.AsyncHandlers.Add<Content, unit> (Content.JsonEncoder, Content.JsonDecoder, E.unit, D.unit, "set")
    member this.LocalClipboardProps : LocalClipboardProps = this.Properties
    member __.OnChanged : IChannel<Content> = onChanged
    member __.GetAsync : IAsyncHandler<unit, Content> = getAsync
    member __.SetAsync : IAsyncHandler<Content, unit> = setAsync
    interface ILocalClipboard with
        member this.LocalClipboardProps : LocalClipboardProps = this.Properties
        member __.OnChanged : IChannel<Content> = onChanged
        member __.GetAsync : IAsyncHandler<unit, Content> = getAsync
        member __.SetAsync : IAsyncHandler<Content, unit> = setAsync
    member this.AsLocalClipboard = this :> ILocalClipboard

(*
 * Generated: <Pack>
 *)
type ILocalPackArgs =
    inherit ITickingPackArgs
    abstract LocalClipboard : Context.Args<ILocalClipboard> with get
    abstract AsTickingPackArgs : ITickingPackArgs with get

type ILocalPack =
    inherit IPack
    inherit ITickingPack
    abstract Args : ILocalPackArgs with get
    abstract LocalClipboard : Context.Agent<ILocalClipboard> with get
    abstract AsTickingPack : ITickingPack with get