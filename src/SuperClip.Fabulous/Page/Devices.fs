[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Devices

open Xamarin.Forms
open Fabulous.DynamicViews

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
module SessionTypes = SuperClip.App.Session.Types

let renderDevice (device : Device) =
    text_cell {
        text device.Name
        detail device.Guid
    }

let render (runner : View) (model : Model) =
    let session = runner.Pack.Session.Actor.State
    table_view {
        intent TableIntent.Menu
        items [
            yield ("This Device", [
                match session.Auth with
                | Some auth ->
                    yield renderDevice auth.Device
                | None ->
                    yield text_cell {
                        text "Offline"
                    }
            ])
            let devices =
                match session.Channel with
                | Some channel ->
                    channel.Devices
                | None -> []
            yield ("Other Online Devices",
                devices |> List.map renderDevice
            )
        ]
    }|> contentPage "Devices"
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem "Help" Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpDevices
            )
        ])
    )
