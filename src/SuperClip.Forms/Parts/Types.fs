[<AutoOpen>]
module SuperClip.Forms.Parts.Types

open FSharp.Control.Tasks.V2
open Xamarin.Forms

open Dap.Platform
open Dap.Remote

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types

module SessionTypes = SuperClip.Forms.Session.Types

type CloudStub = IProxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>
type ChannelService = SuperClip.Core.Channel.Service.Service

type Model = {
    Primary : Primary.Service
    History : History.Agent
    Session : SessionTypes.Agent
} with
    member this.CloudStub =
        this.Session.Actor.Args.Stub