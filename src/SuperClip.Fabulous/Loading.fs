module SuperClip.Fabulous.Loading

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Skia
open Dap.Gui
open Dap.Fabulous

let private getBackgroundColor () =
    let theme = Xamarin.Essentials.Preferences.Get (GuiPrefs.Luid_Theme, "")
    let param = if theme = Theme.DarkTheme then Theme.darkParam else Theme.lightParam
    param.Fabulous.Background

type SuperClipLoadingForm (logging : ILogging) =
    inherit LoadingForm (logging, getBackgroundColor ())
    interface IOverride