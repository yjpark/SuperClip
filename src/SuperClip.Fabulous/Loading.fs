[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Loading

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Skia
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Palette

let getBackgroundColor () =
    let theme = GuiPrefs.getTheme ()
    let scheme = if theme = Theme.DarkTheme then Material.DarkScheme else Material.LightScheme
    scheme.Background

type SuperClipLoadingForm (logging : ILogging) =
    inherit LoadingForm (logging, getBackgroundColor ())
    interface IOverride