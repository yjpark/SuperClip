[<RequireQualifiedAccess>]
module SuperClip.Gui.Presenter.Clip

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
open SuperClip.Gui.Prefab

type Prefab = IClip

let private capText (limit : int) (str : string) =
    if str.Length <= limit then
        str
    else
        sprintf "%s\n..." <| str.Substring (0, limit)

let private getText (item : Item) =
    match item.Content with
    | NoContent -> ""
    | Text text -> text
    | Asset url -> url
    |> capText 128

type Presenter (prefab : Prefab, app : IApp, item : Item) =
    inherit BasePresenter<Item, Prefab> (prefab, item)
    do (
        app.SetGuiValue (prefab.Content.Model.Text, getText item)
    )