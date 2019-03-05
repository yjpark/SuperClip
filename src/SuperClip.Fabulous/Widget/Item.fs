[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Widget.Item

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
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
    | Local -> "This Device"
    | Cloud peer -> peer.Device.Name

let private addMenuItem (item : Item) (text : string) (command : Item -> unit) (v : TextCell) =
    let menuItem = new MenuItem (Text = text)
    menuItem.Clicked.Add (fun _ ->
        command item
    )
    v.ContextActions.Add menuItem
    v

let render' (runner : View) (item : Item) : Widget =
    View.StackLayout (
        orientation = StackOrientation.Vertical,
        horizontalOptions = LayoutOptions.FillAndExpand,
        padding = 0.0,
        spacing = 5.0,
        gestureRecognizers = [
            View.TapGestureRecognizer (
                command = (fun () ->
                    runner.React <| SetPrimary item.Content
                )
            )
        ],
        children = [
            View.Label (
                text = getText item,
                fontSize = 18,
                lineBreakMode = LineBreakMode.MiddleTruncation
            )
            View.Label (
                text = getSource item,
                fontSize = 12,
                textColor = Color.Gray,
                lineBreakMode = LineBreakMode.MiddleTruncation
            )
        ]
    )
let render'' (runner : View) (item : Item) : Widget =
    render' runner item

let render (runner : View) (current : Item) (pinned : bool) (item : Item) =
    let color =
        if current = item || current.Hash = item.Hash then
            Color.Blue
        else
            Color.Black
    View.TextCell(
        created = (fun v ->
            if pinned then
                v
                |> addMenuItem item "Unpin" (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoUnpin (item', None)
                )
            else
                v
                |> addMenuItem item "Pin" (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoPin (item', None)
                )
                |> addMenuItem item "Remove" (fun item' ->
                    runner.Pack.History.Post <| HistoryTypes.DoRemove (item', None)
                )
            |> ignore
        ),
        text = getText item,
        textColor = color,
        detail = getSource item,
        detailColor = Color.Gray,
        command = (fun () ->
            runner.React <| SetPrimary item.Content
        )
    )