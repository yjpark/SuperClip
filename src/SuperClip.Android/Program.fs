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
open Dap.Android
open Dap.Fabulous.Android

open SuperClip.App
open SuperClip.Gui
open SuperClip.Fabulous

let useFabulous = true

[<Activity (Label = "SuperClip", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity () =
    inherit FabulousActivity ()
    override this.UseFabulous () = useFabulous
    override this.DoSetup (bundle : Bundle) =
        let param = AndroidParam.Create ("SuperClip.Android", this, backgroundColor = Android.Graphics.Color.Black)
        if useFabulous then
            setFabulousAndroidParam param
            App.RunFabulous ("super-clip-.log")
        else
            //this.SetContentView (Resources.Layout.Main)
            setAndroidParam param
            App.RunGui ("super-clip-.log")
        |> ignore