[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Helper

open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform

module ChannelTypes = SuperClip.Core.Channel.Types

type ICorePack with
    member this.GetChannelAsync (channel : Channel) : Task<ChannelTypes.Agent> = task {
        let! agent, isNew = this.GetChannelAsync channel.Key
        if isNew then
            do! agent.PostAsync <| ChannelTypes.DoInit channel
        return agent
    }
