[<AutoOpen>]
module SuperClip.Server.Packs

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local.Farango
open Dap.Remote.Dashboard
open SuperClip.Core

module CloudHubTypes = SuperClip.Server.CloudHub.Types
module Gateway = Dap.Remote.WebSocketGateway.Gateway

(*
 * Generated: <Pack>
 *)
type ICloudHubPackArgs =
    inherit IServerPackArgs
    abstract CloudHub : NoArgs with get
    abstract CloudHubGateway : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt> with get
    abstract AsServerPackArgs : IServerPackArgs with get

type ICloudHubPack =
    inherit IPack
    inherit IServerPack
    abstract Args : ICloudHubPackArgs with get
    abstract GetCloudHubAsync : Key -> Task<CloudHubTypes.Agent * bool>
    abstract GetCloudHubGatewayAsync : Key -> Task<Gateway.Gateway * bool>
    abstract AsServerPack : IServerPack with get