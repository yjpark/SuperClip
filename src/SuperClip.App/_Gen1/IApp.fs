[<AutoOpen>]
module SuperClip.App.IApp

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local
open SuperClip.Core

module TickerTypes = Dap.Platform.Ticker.Types
module Context = Dap.Platform.Context
module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types
module ChannelTypes = SuperClip.Core.Channel.Types
module Proxy = Dap.Remote.WebSocketProxy.Proxy
module Cloud = SuperClip.Core.Cloud
module PacketClient = Dap.Remote.WebSocketProxy.PacketClient
module SessionTypes = SuperClip.App.Session.Types

(*
 * Generated: <App>
 *)

type AppKinds () =
    static member Ticker (* ITickingPack *) = "Ticker"
    static member LocalClipboard (* ILocalPack *) = "LocalClipboard"
    static member PrimaryClipboard (* ICorePack *) = "Clipboard"
    static member LocalHistory (* ICorePack *) = "History"
    static member History (* ICorePack *) = "History"
    static member Channel (* ICorePack *) = "Channel"
    static member CloudStub (* ICloudStubPack *) = "CloudStub"
    static member PacketClient (* ICloudStubPack *) = "PacketClient"
    static member Environment (* IAppPack *) = "Environment"
    static member Preferences (* IAppPack *) = "Preferences"
    static member SecureStorage (* IAppPack *) = "SecureStorage"
    static member UserPref (* IClientPack *) = "UserPref"
    static member Session (* ISessionPack *) = "Session"
    static member AppGui (* IGuiPack *) = "AppGui"

type AppKeys () =
    static member Ticker (* ITickingPack *) = ""
    static member LocalClipboard (* ILocalPack *) = ""
    static member PrimaryClipboard (* ICorePack *) = "Primary"
    static member LocalHistory (* ICorePack *) = "Local"
    static member CloudStub (* ICloudStubPack *) = ""
    static member Environment (* IAppPack *) = ""
    static member Preferences (* IAppPack *) = ""
    static member SecureStorage (* IAppPack *) = ""
    static member UserPref (* IClientPack *) = ""
    static member Session (* ISessionPack *) = ""
    static member AppGui (* IGuiPack *) = ""

type IApp =
    inherit IApp<IApp>
    inherit ISessionPack
    inherit IGuiPack
    abstract Args : AppArgs with get
    abstract AsSessionPack : ISessionPack with get
    abstract AsGuiPack : IGuiPack with get

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
and AppArgs = {
    Scope : (* AppArgs *) Scope
    Setup : (* AppArgs *) IApp -> unit
    Ticker : (* ITickingPack *) TickerTypes.Args
    LocalClipboard : (* ILocalPack *) Context.Args<ILocalClipboard>
    PrimaryClipboard : (* ICorePack *) PrimaryTypes.Args
    LocalHistory : (* ICorePack *) HistoryTypes.Args
    History : (* ICorePack *) HistoryTypes.Args
    Channel : (* ICorePack *) ChannelTypes.Args
    CloudStub : (* ICloudStubPack *) Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>
    PacketClient : (* ICloudStubPack *) PacketClient.Args
    Environment : (* IAppPack *) Context.Args<IEnvironment>
    Preferences : (* IAppPack *) Context.Args<IPreferences>
    SecureStorage : (* IAppPack *) Context.Args<ISecureStorage>
    UserPref : (* IClientPack *) Context.Args<IUserPref>
    Session : (* ISessionPack *) NoArgs
    AppGui : (* IGuiPack *) Context.Args<IAppGui>
} with
    static member Create
        (
            ?scope : (* AppArgs *) Scope,
            ?setup : (* AppArgs *) IApp -> unit,
            ?ticker : (* ITickingPack *) TickerTypes.Args,
            ?localClipboard : (* ILocalPack *) Context.Args<ILocalClipboard>,
            ?primaryClipboard : (* ICorePack *) PrimaryTypes.Args,
            ?localHistory : (* ICorePack *) HistoryTypes.Args,
            ?history : (* ICorePack *) HistoryTypes.Args,
            ?channel : (* ICorePack *) ChannelTypes.Args,
            ?cloudStub : (* ICloudStubPack *) Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>,
            ?packetClient : (* ICloudStubPack *) PacketClient.Args,
            ?environment : (* IAppPack *) Context.Args<IEnvironment>,
            ?preferences : (* IAppPack *) Context.Args<IPreferences>,
            ?secureStorage : (* IAppPack *) Context.Args<ISecureStorage>,
            ?userPref : (* IClientPack *) Context.Args<IUserPref>,
            ?session : (* ISessionPack *) NoArgs,
            ?appGui : (* IGuiPack *) Context.Args<IAppGui>
        ) : AppArgs =
        {
            Scope = (* AppArgs *) scope
                |> Option.defaultWith (fun () -> NoScope)
            Setup = (* AppArgs *) setup
                |> Option.defaultWith (fun () -> ignore)
            Ticker = (* ITickingPack *) ticker
                |> Option.defaultWith (fun () -> (TickerTypes.Args.Create ()))
            LocalClipboard = (* ILocalPack *) localClipboard
                |> Option.defaultWith (fun () -> Feature.addToAgent<ILocalClipboard>)
            PrimaryClipboard = (* ICorePack *) primaryClipboard
                |> Option.defaultWith (fun () -> (PrimaryTypes.Args.Create ()))
            LocalHistory = (* ICorePack *) localHistory
                |> Option.defaultWith (fun () -> (HistoryTypes.Args.Create ()))
            History = (* ICorePack *) history
                |> Option.defaultWith (fun () -> (HistoryTypes.Args.Create ()))
            Channel = (* ICorePack *) channel
                |> Option.defaultWith (fun () -> (ChannelTypes.Args.Create ()))
            CloudStub = (* ICloudStubPack *) cloudStub
                |> Option.defaultWith (fun () -> (Proxy.args Cloud.StubSpec (getCloudServerUri ()) false (Some 5.000000<second>) true))
            PacketClient = (* ICloudStubPack *) packetClient
                |> Option.defaultWith (fun () -> (PacketClient.args true 1048576 (decodeJsonString Duration.JsonDecoder """0:00:00:05""")))
            Environment = (* IAppPack *) environment
                |> Option.defaultWith (fun () -> Feature.addToAgent<IEnvironment>)
            Preferences = (* IAppPack *) preferences
                |> Option.defaultWith (fun () -> Feature.addToAgent<IPreferences>)
            SecureStorage = (* IAppPack *) secureStorage
                |> Option.defaultWith (fun () -> Feature.addToAgent<ISecureStorage>)
            UserPref = (* IClientPack *) userPref
                |> Option.defaultWith (fun () -> (fun (agent : IAgent) -> UserPref.Create agent.Env.Logging :> IUserPref))
            Session = (* ISessionPack *) session
                |> Option.defaultWith (fun () -> NoArgs)
            AppGui = (* IGuiPack *) appGui
                |> Option.defaultWith (fun () -> Feature.addToAgent<IAppGui>)
        }
    static member SetScope ((* AppArgs *) scope : Scope) (this : AppArgs) =
        {this with Scope = scope}
    static member SetSetup ((* AppArgs *) setup : IApp -> unit) (this : AppArgs) =
        {this with Setup = setup}
    static member SetTicker ((* ITickingPack *) ticker : TickerTypes.Args) (this : AppArgs) =
        {this with Ticker = ticker}
    static member SetLocalClipboard ((* ILocalPack *) localClipboard : Context.Args<ILocalClipboard>) (this : AppArgs) =
        {this with LocalClipboard = localClipboard}
    static member SetPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) (this : AppArgs) =
        {this with PrimaryClipboard = primaryClipboard}
    static member SetLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) (this : AppArgs) =
        {this with LocalHistory = localHistory}
    static member SetHistory ((* ICorePack *) history : HistoryTypes.Args) (this : AppArgs) =
        {this with History = history}
    static member SetChannel ((* ICorePack *) channel : ChannelTypes.Args) (this : AppArgs) =
        {this with Channel = channel}
    static member SetCloudStub ((* ICloudStubPack *) cloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) (this : AppArgs) =
        {this with CloudStub = cloudStub}
    static member SetPacketClient ((* ICloudStubPack *) packetClient : PacketClient.Args) (this : AppArgs) =
        {this with PacketClient = packetClient}
    static member SetEnvironment ((* IAppPack *) environment : Context.Args<IEnvironment>) (this : AppArgs) =
        {this with Environment = environment}
    static member SetPreferences ((* IAppPack *) preferences : Context.Args<IPreferences>) (this : AppArgs) =
        {this with Preferences = preferences}
    static member SetSecureStorage ((* IAppPack *) secureStorage : Context.Args<ISecureStorage>) (this : AppArgs) =
        {this with SecureStorage = secureStorage}
    static member SetUserPref ((* IClientPack *) userPref : Context.Args<IUserPref>) (this : AppArgs) =
        {this with UserPref = userPref}
    static member SetSession ((* ISessionPack *) session : NoArgs) (this : AppArgs) =
        {this with Session = session}
    static member SetAppGui ((* IGuiPack *) appGui : Context.Args<IAppGui>) (this : AppArgs) =
        {this with AppGui = appGui}
    static member JsonEncoder : JsonEncoder<AppArgs> =
        fun (this : AppArgs) ->
            E.object [
                "scope", Scope.JsonEncoder (* AppArgs *) this.Scope
                "ticker", TickerTypes.Args.JsonEncoder (* ITickingPack *) this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder (* ICorePack *) this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.History
                "channel", ChannelTypes.Args.JsonEncoder (* ICorePack *) this.Channel
            ]
    static member JsonDecoder : JsonDecoder<AppArgs> =
        D.object (fun get ->
            {
                Scope = get.Optional.Field (* AppArgs *) "scope" Scope.JsonDecoder
                    |> Option.defaultValue NoScope
                Setup = (* (* AppArgs *)  *) ignore
                Ticker = get.Optional.Field (* ITickingPack *) "ticker" TickerTypes.Args.JsonDecoder
                    |> Option.defaultValue (TickerTypes.Args.Create ())
                LocalClipboard = (* (* ILocalPack *)  *) Feature.addToAgent<ILocalClipboard>
                PrimaryClipboard = get.Optional.Field (* ICorePack *) "primary_clipboard" PrimaryTypes.Args.JsonDecoder
                    |> Option.defaultValue (PrimaryTypes.Args.Create ())
                LocalHistory = get.Optional.Field (* ICorePack *) "local_history" HistoryTypes.Args.JsonDecoder
                    |> Option.defaultValue (HistoryTypes.Args.Create ())
                History = get.Optional.Field (* ICorePack *) "history" HistoryTypes.Args.JsonDecoder
                    |> Option.defaultValue (HistoryTypes.Args.Create ())
                Channel = get.Optional.Field (* ICorePack *) "channel" ChannelTypes.Args.JsonDecoder
                    |> Option.defaultValue (ChannelTypes.Args.Create ())
                CloudStub = (* (* ICloudStubPack *)  *) (Proxy.args Cloud.StubSpec (getCloudServerUri ()) false (Some 5.000000<second>) true)
                PacketClient = (* (* ICloudStubPack *)  *) (PacketClient.args true 1048576 (decodeJsonString Duration.JsonDecoder """0:00:00:05"""))
                Environment = (* (* IAppPack *)  *) Feature.addToAgent<IEnvironment>
                Preferences = (* (* IAppPack *)  *) Feature.addToAgent<IPreferences>
                SecureStorage = (* (* IAppPack *)  *) Feature.addToAgent<ISecureStorage>
                UserPref = (* (* IClientPack *)  *) (fun (agent : IAgent) -> UserPref.Create agent.Env.Logging :> IUserPref)
                Session = (* (* ISessionPack *)  *) NoArgs
                AppGui = (* (* IGuiPack *)  *) Feature.addToAgent<IAppGui>
            }
        )
    static member JsonSpec =
        FieldSpec.Create<AppArgs> (AppArgs.JsonEncoder, AppArgs.JsonDecoder)
    interface IJson with
        member this.ToJson () = AppArgs.JsonEncoder this
    interface IObj
    member this.WithScope ((* AppArgs *) scope : Scope) =
        this |> AppArgs.SetScope scope
    member this.WithSetup ((* AppArgs *) setup : IApp -> unit) =
        this |> AppArgs.SetSetup setup
    member this.WithTicker ((* ITickingPack *) ticker : TickerTypes.Args) =
        this |> AppArgs.SetTicker ticker
    member this.WithLocalClipboard ((* ILocalPack *) localClipboard : Context.Args<ILocalClipboard>) =
        this |> AppArgs.SetLocalClipboard localClipboard
    member this.WithPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        this |> AppArgs.SetPrimaryClipboard primaryClipboard
    member this.WithLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) =
        this |> AppArgs.SetLocalHistory localHistory
    member this.WithHistory ((* ICorePack *) history : HistoryTypes.Args) =
        this |> AppArgs.SetHistory history
    member this.WithChannel ((* ICorePack *) channel : ChannelTypes.Args) =
        this |> AppArgs.SetChannel channel
    member this.WithCloudStub ((* ICloudStubPack *) cloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) =
        this |> AppArgs.SetCloudStub cloudStub
    member this.WithPacketClient ((* ICloudStubPack *) packetClient : PacketClient.Args) =
        this |> AppArgs.SetPacketClient packetClient
    member this.WithEnvironment ((* IAppPack *) environment : Context.Args<IEnvironment>) =
        this |> AppArgs.SetEnvironment environment
    member this.WithPreferences ((* IAppPack *) preferences : Context.Args<IPreferences>) =
        this |> AppArgs.SetPreferences preferences
    member this.WithSecureStorage ((* IAppPack *) secureStorage : Context.Args<ISecureStorage>) =
        this |> AppArgs.SetSecureStorage secureStorage
    member this.WithUserPref ((* IClientPack *) userPref : Context.Args<IUserPref>) =
        this |> AppArgs.SetUserPref userPref
    member this.WithSession ((* ISessionPack *) session : NoArgs) =
        this |> AppArgs.SetSession session
    member this.WithAppGui ((* IGuiPack *) appGui : Context.Args<IAppGui>) =
        this |> AppArgs.SetAppGui appGui
    interface ITickingPackArgs with
        member this.Ticker (* ITickingPack *) : TickerTypes.Args = this.Ticker
    member this.AsTickingPackArgs = this :> ITickingPackArgs
    interface ILocalPackArgs with
        member this.LocalClipboard (* ILocalPack *) : Context.Args<ILocalClipboard> = this.LocalClipboard
        member this.AsTickingPackArgs = this.AsTickingPackArgs
    member this.AsLocalPackArgs = this :> ILocalPackArgs
    interface ICorePackArgs with
        member this.PrimaryClipboard (* ICorePack *) : PrimaryTypes.Args = this.PrimaryClipboard
        member this.LocalHistory (* ICorePack *) : HistoryTypes.Args = this.LocalHistory
        member this.History (* ICorePack *) : HistoryTypes.Args = this.History
        member this.Channel (* ICorePack *) : ChannelTypes.Args = this.Channel
        member this.AsLocalPackArgs = this.AsLocalPackArgs
    member this.AsCorePackArgs = this :> ICorePackArgs
    interface ICloudStubPackArgs with
        member this.CloudStub (* ICloudStubPack *) : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt> = this.CloudStub
        member this.PacketClient (* ICloudStubPack *) : PacketClient.Args = this.PacketClient
        member this.AsTickingPackArgs = this.AsTickingPackArgs
    member this.AsCloudStubPackArgs = this :> ICloudStubPackArgs
    interface IAppPackArgs with
        member this.Environment (* IAppPack *) : Context.Args<IEnvironment> = this.Environment
        member this.Preferences (* IAppPack *) : Context.Args<IPreferences> = this.Preferences
        member this.SecureStorage (* IAppPack *) : Context.Args<ISecureStorage> = this.SecureStorage
    member this.AsAppPackArgs = this :> IAppPackArgs
    interface IClientPackArgs with
        member this.UserPref (* IClientPack *) : Context.Args<IUserPref> = this.UserPref
        member this.AsCorePackArgs = this.AsCorePackArgs
        member this.AsCloudStubPackArgs = this.AsCloudStubPackArgs
        member this.AsAppPackArgs = this.AsAppPackArgs
    member this.AsClientPackArgs = this :> IClientPackArgs
    interface ISessionPackArgs with
        member this.Session (* ISessionPack *) : NoArgs = this.Session
        member this.AsClientPackArgs = this.AsClientPackArgs
    member this.AsSessionPackArgs = this :> ISessionPackArgs
    interface IGuiPackArgs with
        member this.AppGui (* IGuiPack *) : Context.Args<IAppGui> = this.AppGui
    member this.AsGuiPackArgs = this :> IGuiPackArgs

(*
 * Generated: <ValueBuilder>
 *)
type AppArgsBuilder () =
    inherit ObjBuilder<AppArgs> ()
    override __.Zero () = AppArgs.Create ()
    [<CustomOperation("scope")>]
    member __.Scope (target : AppArgs, (* AppArgs *) scope : Scope) =
        target.WithScope scope
    [<CustomOperation("Setup")>]
    member __.Setup (target : AppArgs, (* AppArgs *) setup : IApp -> unit) =
        target.WithSetup setup
    [<CustomOperation("ticker")>]
    member __.Ticker (target : AppArgs, (* ITickingPack *) ticker : TickerTypes.Args) =
        target.WithTicker ticker
    [<CustomOperation("local_clipboard")>]
    member __.LocalClipboard (target : AppArgs, (* ILocalPack *) localClipboard : Context.Args<ILocalClipboard>) =
        target.WithLocalClipboard localClipboard
    [<CustomOperation("primary_clipboard")>]
    member __.PrimaryClipboard (target : AppArgs, (* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        target.WithPrimaryClipboard primaryClipboard
    [<CustomOperation("local_history")>]
    member __.LocalHistory (target : AppArgs, (* ICorePack *) localHistory : HistoryTypes.Args) =
        target.WithLocalHistory localHistory
    [<CustomOperation("history")>]
    member __.History (target : AppArgs, (* ICorePack *) history : HistoryTypes.Args) =
        target.WithHistory history
    [<CustomOperation("channel")>]
    member __.Channel (target : AppArgs, (* ICorePack *) channel : ChannelTypes.Args) =
        target.WithChannel channel
    [<CustomOperation("cloud_stub")>]
    member __.CloudStub (target : AppArgs, (* ICloudStubPack *) cloudStub : Proxy.Args<Cloud.Req, Cloud.ClientRes, Cloud.Evt>) =
        target.WithCloudStub cloudStub
    [<CustomOperation("packet_client")>]
    member __.PacketClient (target : AppArgs, (* ICloudStubPack *) packetClient : PacketClient.Args) =
        target.WithPacketClient packetClient
    [<CustomOperation("environment")>]
    member __.Environment (target : AppArgs, (* IAppPack *) environment : Context.Args<IEnvironment>) =
        target.WithEnvironment environment
    [<CustomOperation("preferences")>]
    member __.Preferences (target : AppArgs, (* IAppPack *) preferences : Context.Args<IPreferences>) =
        target.WithPreferences preferences
    [<CustomOperation("secure_storage")>]
    member __.SecureStorage (target : AppArgs, (* IAppPack *) secureStorage : Context.Args<ISecureStorage>) =
        target.WithSecureStorage secureStorage
    [<CustomOperation("user_pref")>]
    member __.UserPref (target : AppArgs, (* IClientPack *) userPref : Context.Args<IUserPref>) =
        target.WithUserPref userPref
    [<CustomOperation("session")>]
    member __.Session (target : AppArgs, (* ISessionPack *) session : NoArgs) =
        target.WithSession session
    [<CustomOperation("app_gui")>]
    member __.AppGui (target : AppArgs, (* IGuiPack *) appGui : Context.Args<IAppGui>) =
        target.WithAppGui appGui

let app_args = new AppArgsBuilder ()