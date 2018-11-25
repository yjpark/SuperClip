[<RequireQualifiedAccess>]
module SuperClip.Gui.Presenter.Clips

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
open SuperClip.Gui.Prefab

module HistoryTypes = SuperClip.Core.History.Types

type Prefab = IClips

type Presenter (prefab : Prefab, app : IApp) =
    inherit DynamicPresenter<Item list, Prefab> (prefab)
    override this.OnWillAttach (items : Item list) =
        prefab.ResizeItems items.Length
        (items, prefab.Target.Prefabs0)
        ||> List.iter2 (fun item itemPrefab ->
            new Clip.Presenter(itemPrefab :?> IClip, app, item) |> ignore
        )