[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Devices

open Xamarin.Forms
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let renderDevice (runner : View) (device : Device) =
    View.TextCell (
        text = device.Name,
        textColor = Color.Black,
        detail = device.Guid,
        detailColor = Color.Gray
    )

let render (runner : View) (model : Model) =
    let session = runner.Pack.Session.Actor.State
    let view = View.NonScrollingContentPage (
        "Devices",
        [
            View.TableView (
                intent = TableIntent.Menu,
                items = [
                    match session.Channel with
                    | Some channel ->
                        yield ("Other Online Devices",
                            channel.Devices
                            |> List.map ^<| renderDevice runner
                        )
                    | None ->
                        yield ("Not Online", [])
                ]
            )
        ]
    )
    view.ToolbarItems([
        yield toolbarItem "Help" (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpDevices
        )
    ])
