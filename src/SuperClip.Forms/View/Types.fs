[<AutoOpen>]
module SuperClip.Forms.View.Types

open Xamarin.Forms

open Dap.Platform
open Dap.Remote

module ViewTypes = Dap.Forms.View.Types

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service

module CloudTypes = SuperClip.Core.Cloud.Types

type CloudStub = IProxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>

type Args = ViewTypes.Args<Model, Msg>

and Model = {
    Primary : Primary.Service
    History : History.Agent
    CloudStub : CloudStub
    CloudPeers : Peers option
}

and Msg =
    | SetPrimary of Content
    | PrimaryEvt of Clipboard.Evt
    | CloudEvt of CloudTypes.Evt
    | CloudRes of CloudTypes.ClientRes
with interface IMsg

and View = ViewTypes.View<Model, Msg>
and Initer = ViewTypes.Initer<Model, Msg>
and Render = ViewTypes.Render<Model, Msg>