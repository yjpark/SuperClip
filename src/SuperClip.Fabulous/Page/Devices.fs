[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Devices

open Xamarin.Forms
open Fabulous.Core
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Fabulous

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let renderDevice (runner : View) (device : Device) =
    View.TextCell (
        text = device.Name,
        detail = device.Guid,
        created = Theme.decorate
    )

let render (runner : View) (model : Model) =
    let session = runner.Pack.Session.Actor.State
    let view = View.NonScrollingContentPage (
        "Devices",
        [
            View.TableView (
                intent = TableIntent.Menu,
                items = [
                    yield ("This Device", [
                        match session.Auth with
                        | Some auth ->
                            yield renderDevice runner auth.Device
                        | None ->
                            yield View.TextCell (
                                text = "Offline",
                                created = Theme.decorate
                            )
                    ])
                    let devices =
                        match session.Channel with
                        | Some channel ->
                            channel.Devices
                        | None -> []
                    yield ("Other Online Devices",
                        devices |> List.map ^<| renderDevice runner
                    )
                ],
                created = Theme.decorate
            )
        ]
    )
    view.ToolbarItems([
        yield toolbarItem "Help" (fun () ->
            runner.React <| DoSetHelp ^<| Some HelpDevices
        )
    ])
