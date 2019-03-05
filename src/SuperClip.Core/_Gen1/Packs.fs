[<AutoOpen>]
module SuperClip.Core.Packs

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types

(*
 * Generated: <Pack>
 *)
type ICorePackArgs =
    inherit ILocalPackArgs
    abstract PrimaryClipboard : PrimaryTypes.Args with get
    abstract LocalHistory : HistoryTypes.Args with get
    abstract History : HistoryTypes.Args with get
    abstract AsLocalPackArgs : ILocalPackArgs with get

type ICorePack =
    inherit IPack
    inherit ILocalPack
    abstract Args : ICorePackArgs with get
    abstract PrimaryClipboard : PrimaryTypes.Agent with get
    abstract LocalHistory : HistoryTypes.Agent with get
    abstract GetHistoryAsync : Key -> Task<HistoryTypes.Agent * bool>
    abstract AsLocalPack : ILocalPack with get