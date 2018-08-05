[<AutoOpen>]
module SuperClip.Forms.Types

open Xamarin.Forms

open Dap.Platform
open Dap.Remote

module AppTypes = Dap.Forms.App.Types
module AppHelper = Dap.Forms.App.Helper

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service

module CloudTypes = SuperClip.Core.Cloud.Types

let CloudUri = "ws://localhost:5700/ws_user"

type CloudStub = IProxy<CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt>

type AppArgs = AppTypes.Args<App, AppModel, AppMsg>

and AppModel = {
    Primary : Primary.Service
    History : History.Agent
    CloudStub : CloudStub
}

and AppMsg =
    | SetPrimary of Content
    | PrimaryEvt of Clipboard.Evt
    | CloudEvt of CloudTypes.Evt
    | CloudRes of CloudTypes.ClientRes
with interface IMsg

and AppIniter = AppTypes.AppIniter<AppModel, AppMsg>
and AppView = AppTypes.AppView<App, AppModel, AppMsg>

and App (param) =
    inherit AppTypes.App<App, AppModel, AppMsg> (param)
    static member Spawn (param) = new App (param)
    override this.Runner = this