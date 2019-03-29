[<RequireQualifiedAccess>]
module SuperClip.Fabulous.GuiPrefs

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local
open Dap.Gui

open SuperClip.App
open SuperClip.Fabulous.View.Types
open SuperClip.Core

[<Literal>]
let Luid_CloudMode = "cloud_mode"

[<Literal>]
let Luid_CloudServerUri = "cloud_server_uri"

[<Literal>]
let Luid_SeparateSyncing = "separate_syncing"

[<Literal>]
let Luid_Theme = "theme"

[<Literal>]
let Luid_Locale = "locale"

[<Literal>]
let Luid_AuthDevice = "auth_device"
[<Literal>]
let Luid_AuthChannel= "auth_channel"

let getCloudMode () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_CloudMode)
    |> Option.map (fun m -> m = "true")
    |> Option.defaultValue true

let setCloudMode (cloudMode : bool) =
    let v = if cloudMode then "true" else "false"
    SetTextReq.Create (path = Luid_CloudMode, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getCloudServerUri () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_CloudServerUri)
    |> Option.bind (fun url ->
        if System.String.IsNullOrEmpty url then
            None
        else
            Some url
    )|> Option.defaultWith getDefaultCloudServerUri

let isDefaultCloudServerUri () =
    getCloudServerUri () = getDefaultCloudServerUri ()

let setCloudServerUri (uri : string) =
    SetTextReq.Create (path = Luid_CloudServerUri, text = uri)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getSeparateSyncing () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_SeparateSyncing)
    |> Option.map (fun m -> m = "true")
    |> Option.defaultValue false

let setSeparateSyncing (v : bool) =
    let v = if v then "true" else "false"
    SetTextReq.Create (path = Luid_SeparateSyncing, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getTheme () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_Theme)
    |> Option.defaultValue Theme.LightTheme

let setTheme (v : string) =
    SetTextReq.Create (path = Luid_Theme, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getLocale () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_Locale)
    |> Option.defaultValue Locale.DefaultLocale

let setLocale (v : string) =
    SetTextReq.Create (path = Luid_Locale, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getAuthDevice () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_AuthDevice)
    |> Option.bind (fun device ->
        if System.String.IsNullOrEmpty device then
            None
        else
            Some device
    )|> Option.defaultWith getDeviceName

let setAuthDevice (v : string) =
    SetTextReq.Create (path = Luid_AuthDevice, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

let getAuthChannel () =
    IEnvironment.Instance.Preferences.Get.Handle (Luid_AuthChannel)
    |> Option.defaultValue ""

let setAuthChannel (v : string) =
    SetTextReq.Create (path = Luid_AuthChannel, text = v)
    |> IEnvironment.Instance.Preferences.Set.Handle

