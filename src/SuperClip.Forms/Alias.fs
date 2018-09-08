[<AutoOpen>]
module SuperClip.Forms.Alias

open FSharp.Control.Tasks.V2
open Xamarin.Forms

open Dap.Platform
open Dap.Remote

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types

type CloudStub = SuperClip.Forms.Parts.Types.CloudStub

type Parts = SuperClip.Forms.Parts.Types.Model