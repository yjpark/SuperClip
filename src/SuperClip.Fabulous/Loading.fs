[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Loading

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Skia
open Dap.Gui
open Dap.Fabulous

let getTheme () =
    let theme = Xamarin.Essentials.Preferences.Get (GuiPrefs.Luid_Theme, "")
    if theme = Theme.DarkTheme then Theme.DarkTheme else Theme.LightTheme

let getBackgroundColor () =
    let theme = getTheme ()
    let param = if theme = Theme.DarkTheme then Theme.darkParam else Theme.lightParam
    param.Fabulous.Background

type SuperClipLoadingForm (logging : ILogging) =
    inherit LoadingForm (logging, getBackgroundColor ())
    interface IOverride