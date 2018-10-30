[<RequireQualifiedAccess>]
module SuperClip.Presenter.AuthPanel

open Dap.Prelude
open Dap.Context
open Dap.Gui

open SuperClip.Core
open SuperClip.App

type Prefab = SuperClip.Prefab.AuthPanel.Prefab

type Presenter (app : IApp) =
    inherit BasePresenter<Prefab> (new Prefab (app.Env.Logging))