[<AutoOpen>]
module SuperClip.App.Packs

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local
open SuperClip.Core

module SessionTypes = SuperClip.App.Session.Types

(*
 * Generated: <Pack>
 *)
type ISessionPackArgs =
    inherit IClientPackArgs
    abstract Session : NoArgs with get
    abstract AsClientPackArgs : IClientPackArgs with get

type ISessionPack =
    inherit IPack
    inherit IClientPack
    abstract Args : ISessionPackArgs with get
    abstract Session : SessionTypes.Agent with get
    abstract AsClientPack : IClientPack with get