module SuperClip.Android.Program

open System
open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
open Xamarin.Forms.Platform.Android

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Android
open Dap.Fabulous.Android

open SuperClip.App
open SuperClip.Gui
open SuperClip.Fabulous

let useFabulous = true

[<Activity (Label = "SuperClip", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity () as self =
    inherit FabulousActivity ()
    static let mutable instance : MainActivity option = None
    do (
        instance <- Some self
    )
    static member Instance = instance.Value
    override this.UseFabulous () = useFabulous
    override this.OnCreate (bundle: Bundle) =
        let view = this.Window.DecorView
        view.SetBackgroundColor (Android.Graphics.Color.Black)
        base.OnCreate (bundle)
    override this.DoSetup (bundle : Bundle) =
        let param = AndroidParam.Create ("SuperClip.Android", this)
        if useFabulous then
            setFabulousAndroidParam param
            App.RunFabulous ("super-clip-.log")
        else
            //this.SetContentView (Resources.Layout.Main)
            setAndroidParam param
            App.RunGui ("super-clip-.log")
        |> ignore

type AndroidThemeHook (logging : ILogging) =
    inherit EmptyContext(logging, "AndroidThemeHook")
    interface IGuiAppHook with
        member this.OnInit (app : IGuiApp) =
            app.OnWillSwitchTheme.AddWatcher this "OnWillSwitchTheme" (fun theme ->
                if theme.Key = Theme.DarkTheme then
                    MainActivity.Instance.SwitchDarkTheme ()
                else
                    MainActivity.Instance.SwitchLightTheme ()
            )

