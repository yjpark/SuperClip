[<AutoOpen>]
module SuperClip.Core.Packs

open Dap.Context.Helper
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types

type ICorePackArgs =
    inherit IServicesPackArgs
    abstract PrimaryClipboard : PrimaryTypes.Args with get
    abstract LocalHistory : HistoryTypes.Args with get
    abstract History : HistoryTypes.Args with get
    abstract AsServicesPackArgs : IServicesPackArgs with get

type ICorePack =
    inherit IPack
    inherit IServicesPack
    abstract Args : ICorePackArgs with get
    abstract PrimaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> with get
    abstract LocalHistory : HistoryTypes.Agent with get
    abstract GetHistoryAsync : Key -> Task<HistoryTypes.Agent * bool>
    abstract AsServicesPack : IServicesPack with get