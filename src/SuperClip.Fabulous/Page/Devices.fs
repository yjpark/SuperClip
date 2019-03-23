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
            yield (Locale.Text.Common.ThisDevice, [
                match session.Auth with
                | Some auth ->
                    yield renderDevice auth.Device
                | None ->
                    yield text_cell {
                        text Locale.Text.Devices.Offline
                    }
            ])
            let devices =
                match session.Channel with
                | Some channel ->
                    channel.Devices
                | None -> []
            yield (Locale.Text.Devices.OtherOnlineDevices,
                devices |> List.map renderDevice
            )
        ]
    }|> contentPage Locale.Text.Devices.Title
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem Locale.Text.Help.Title Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpDevices
            )
        ])
    )
