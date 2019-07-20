module SuperClip.iOS.Program

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.iOS
open Dap.Fabulous.iOS
open Dap.Gui

open SuperClip.App
open SuperClip.Fabulous

type IOSThemeHook (logging : ILogging) =
    inherit EmptyContext(logging, "AndroidThemeHook")
    interface IGuiAppHook with
        member this.OnInit (app : IGuiApp) =
            ()
            (*
            app.OnWillSwitchTheme.AddWatcher this "OnWillSwitchTheme" (fun theme ->
                if theme.Key = Theme.DarkTheme then
                    MainActivity.Instance.SwitchDarkTheme ()
                else
                    MainActivity.Instance.SwitchLightTheme ()
            )
            *)

[<EntryPoint>]
[<STAThread>]
let main args =
    setFabulousIOSParam <| IOSParam.Create ("SuperClip.iOS")
    App.RunFabulous ("super-clip-.log")


