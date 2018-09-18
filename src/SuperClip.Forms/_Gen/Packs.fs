[<AutoOpen>]
module SuperClip.Forms.Packs

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open SuperClip.Core

module SessionTypes = SuperClip.Forms.Session.Types

type ISessionPackArgs =
    inherit IClientPackArgs
    abstract Session : NoArgs with get

type ISessionPack =
    inherit IPack
    inherit IClientPack
    abstract Args : ISessionPackArgs with get
    abstract Session : SessionTypes.Agent with get