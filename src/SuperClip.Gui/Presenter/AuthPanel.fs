[<RequireQualifiedAccess>]
module SuperClip.Gui.Presenter.AuthPanel

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
open SuperClip.Gui.Prefab

type Prefab = IAuthPanel

type Presenter (env : IEnv) =
    inherit BasePresenter<Prefab, IApp> (Feature.create<Prefab> env.Logging)