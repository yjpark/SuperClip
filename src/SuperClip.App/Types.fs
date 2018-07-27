[<AutoOpen>]
module SuperClip.App.Types

open Xamarin.Forms

open Dap.Platform
module AppTypes = Dap.Forms.App.Types
module AppHelper = Dap.Forms.App.Helper

open SuperClip.Core
module History = SuperClip.Core.History.Agent
module Primary = SuperClip.Core.Primary.Service

type AppArgs = AppTypes.Args<App, AppModel, AppMsg>

and AppModel = {
    Primary : Primary.Service
    History : History.Agent
}

and AppMsg =
    | SetPrimary of Clipboard.Content
    | PrimaryEvt of Clipboard.Evt
with interface IMsg

and AppIniter = AppTypes.AppIniter<AppModel, AppMsg>
and AppView = AppTypes.AppView<App, AppModel, AppMsg>

and App (param) =
    inherit AppTypes.App<App, AppModel, AppMsg> (param)
    let app = AppHelper.newApplication ()
    static member Spawn (param) = new App (param)
    static member CreateAsync args consoleLogLevel logFile =
        AppHelper.createAsync App.Spawn args "SuperClip" consoleLogLevel logFile
    override this.Runner = this
    override _this.Application = app