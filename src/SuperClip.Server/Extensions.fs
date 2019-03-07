[<AutoOpen>]
module SuperClip.Server.Extensions

open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform
open Dap.Local
open Dap.Local.Farango

open SuperClip.Core

module ChannelTypes = SuperClip.Core.Channel.Types

type IServerPack with
    member this.GetChannelAsync (channel : Channel) : Task<ChannelTypes.Agent> = task {
        let! agent, isNew = this.GetChannelAsync channel.Key
        if isNew then
            do! agent.PostAsync <| ChannelTypes.DoInit channel
        return agent
    }

