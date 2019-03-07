[<AutoOpen>]
module SuperClip.Server.Types

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

module ChannelTypes = SuperClip.Core.Channel.Types

(*
 * Generated: <Pack>
 *)
type IServerPackArgs =
    inherit IDbPackArgs
    abstract Channel : ChannelTypes.Args with get
    abstract AsDbPackArgs : IDbPackArgs with get

type IServerPack =
    inherit IPack
    inherit IDbPack
    abstract Args : IServerPackArgs with get
    abstract GetChannelAsync : Key -> Task<ChannelTypes.Agent * bool>
    abstract AsDbPack : IDbPack with get