[<RequireQualifiedAccess>]
module SuperClip.Presenter.HomePanel

open Dap.Prelude
open Dap.Context
open Dap.Gui

open SuperClip.Core
open SuperClip.App

type Prefab = SuperClip.Prefab.HomePanel.Prefab

type Presenter (app : IApp) =
    inherit BasePresenter<Prefab> (new Prefab (app.Env.Logging))
    let linkStatus = new SuperClip.Presenter.LinkStatus.Presenter (app, base.Prefab.LinkStatus)