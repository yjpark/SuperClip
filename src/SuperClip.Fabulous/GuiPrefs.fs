[<RequireQualifiedAccess>]
module SuperClip.Fabulous.GuiPrefs

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local
open Dap.Gui

open SuperClip.App
open SuperClip.Fabulous.View.Types

[<Literal>]
let Luid_CloudMode = "cloud_mode"

[<Literal>]
let Luid_Theme = "theme"

[<Literal>]
let Luid_Locale = "locale"

[<Literal>]
let Luid_AuthDevice = "auth_device"
[<Literal>]
let Luid_AuthChannel= "auth_channel"

let getCloudMode (runner : View) =
    runner.Pack.Preferences.Context.Get.Handle (Luid_CloudMode)
    |> Option.map (fun m -> m = "true")
    |> Option.defaultValue true

let setCloudMode (cloudMode : bool) (runner : View) =
    let v = if cloudMode then "true" else "false"
    SetTextReq.Create (path = Luid_CloudMode, text = v)
    |> runner.Pack.Preferences.Context.Set.Handle

let getTheme (runner : View) =
    runner.Pack.Preferences.Context.Get.Handle (Luid_Theme)
    |> Option.defaultValue Theme.LightTheme

let setTheme (v : string) (runner : View) =
    SetTextReq.Create (path = Luid_Theme, text = v)
    |> runner.Pack.Preferences.Context.Set.Handle

let getLocale (runner : View) =
    runner.Pack.Preferences.Context.Get.Handle (Luid_Locale)
    |> Option.defaultValue Locale.DefaultLocale

let setLocale (v : string) (runner : View) =
    SetTextReq.Create (path = Luid_Locale, text = v)
    |> runner.Pack.Preferences.Context.Set.Handle

let getAuthDevice (runner : View) =
    runner.Pack.Preferences.Context.Get.Handle (Luid_AuthDevice)
    |> Option.bind (fun device ->
        if System.String.IsNullOrEmpty device then
            None
        else
            Some device
    )|> Option.defaultWith getDeviceName

let setAuthDevice (v : string) (runner : View) =
    SetTextReq.Create (path = Luid_AuthDevice, text = v)
    |> runner.Pack.Preferences.Context.Set.Handle

let getAuthChannel (runner : View) =
    runner.Pack.Preferences.Context.Get.Handle (Luid_AuthChannel)
    |> Option.defaultValue ""

let setAuthChannel (v : string) (runner : View) =
    SetTextReq.Create (path = Luid_AuthChannel, text = v)
    |> runner.Pack.Preferences.Context.Set.Handle

