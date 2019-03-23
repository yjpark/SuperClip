[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Widget.Item

open Xamarin.Forms

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Fabulous
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types

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
    |> fun t -> t.Trim ()
    |> capText 128

let private getSource (item : Item) =
    match item.Source with
    | NoSource -> ""
    | Local -> Locale.Text.Common.ThisDevice
    | Cloud peer -> peer.Device.Name

let private addMenuItem (item : Item) (text : string) (command : Item -> unit) (v : TextCell) =
    let menuItem = new MenuItem (Text = text)
    menuItem.Clicked.Add (fun _ ->
        command item
    )
    v.ContextActions.Add menuItem
    v

let render (runner : View) (current : Item) (pinned : bool) (item : Item) =
    let isCurrent =
        current = item || current.Hash = item.Hash
    text_cell {
        classId (if isCurrent then Theme.TextCell_Current else "")
        text (getText item)
        detail (getSource item)
        command (fun () ->
            runner.React <| SetPrimary item.Content
        )
        callback (fun cell ->
            if pinned then
                cell
                |> addMenuItem item Locale.Text.Item.Unpin (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoUnpin (item', None)
                )
            else
                cell
                |> addMenuItem item Locale.Text.Item.Pin (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoPin (item', None)
                )
                |> addMenuItem item Locale.Text.Item.Remove (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoRemove (item', None)
                )
            |> ignore
        )
    }