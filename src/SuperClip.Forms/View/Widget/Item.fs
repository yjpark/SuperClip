[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Widget.Item

open Xamarin.Forms
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
open SuperClip.Forms
open SuperClip.Forms.View.Types
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
    |> capText 128

let render (runner : View) (item : Item) : Widget =
    View.Button (
        text = getText item,
        horizontalOptions = LayoutOptions.FillAndExpand,
        verticalOptions = LayoutOptions.Center,
        fontSize = "Large",
        command = (fun () ->
            runner.React <| SetPrimary item.Content
        )
    )