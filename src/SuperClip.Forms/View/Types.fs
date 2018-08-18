[<AutoOpen>]
module SuperClip.Forms.View.Types

open Xamarin.Forms

open Dap.Platform
open Dap.Remote

module ViewTypes = Dap.Forms.View.Types

open SuperClip.Core
open SuperClip.Forms
module Primary = SuperClip.Core.Primary.Service
module CloudTypes = SuperClip.Core.Cloud.Types
module HistoryTypes = SuperClip.Core.History.Types

type Parts = SuperClip.Forms.Parts.Types.Model

type Args = ViewTypes.Args<Model, Msg>

and Model = {
    Parts : Parts
} with
    member this.Primary = this.Parts.Primary
    member this.History = this.Parts.History
    member this.CloudStub = this.Parts.CloudStub

and Msg =
    | SetPrimary of Content
    | HistoryEvt of HistoryTypes.Evt
with interface IMsg

and View = ViewTypes.View<Model, Msg>
and Initer = ViewTypes.Initer<Model, Msg>
and Render = ViewTypes.Render<Model, Msg>