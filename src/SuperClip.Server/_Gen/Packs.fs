[<AutoOpen>]
module SuperClip.Server.Packs

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local.Farango
open SuperClip.Core

module PacketConn = Dap.Remote.WebSocketGateway.PacketConn
module CloudHubTypes = SuperClip.Server.CloudHub.Types
module Gateway = Dap.Remote.WebSocketGateway.Gateway

type ICloudHubPackArgs =
    abstract PacketConn : PacketConn.Args with get
    abstract CloudHub : NoArgs with get
    abstract CloudHubGateway : Gateway.Args<CloudHubTypes.Req, CloudHubTypes.Evt> with get

type ICloudHubPack =
    abstract Args : ICloudHubPackArgs with get
    abstract GetPacketConnAsync : Key -> Task<IAgent<PacketConn.Req, PacketConn.Evt> * bool>
    abstract GetCloudHubAsync : Key -> Task<CloudHubTypes.Agent * bool>
    abstract GetCloudHubGatewayAsync : Key -> Task<Gateway.Gateway * bool>