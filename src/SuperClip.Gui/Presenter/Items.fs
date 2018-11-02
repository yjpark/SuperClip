[<RequireQualifiedAccess>]
module SuperClip.Gui.Presenter.Items

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.App
module HistoryTypes = SuperClip.Core.History.Types

type Prefab = ILabel

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

type Presenter (prefab : Prefab, app : IApp) =
    inherit DynamicPresenter<Prefab, Item list> (prefab)
    override this.OnWillAttach (items : Item list) =
        let text =
            match List.tryHead items with
            | None -> ""
            | Some item -> getText item
        app.SetGuiValue (prefab.Model.Text, text)