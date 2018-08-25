[<RequireQualifiedAccess>]
module SuperClip.Forms.View.Widget.Item

open Xamarin.Forms
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Forms
open SuperClip.Forms.View.Types
module HistoryTypes = SuperClip.Core.History.Types
module CloudTypes = SuperClip.Core.Cloud.Types

let private capText (limit : int) (str : string) =
    if str.Length <= limit then
        str
    else
        sprintf "%s\n..." <| str.Substring (0, limit)

let private getText (item : Item) =
    match item.Content with
    | Text text -> text
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