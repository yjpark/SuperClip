// Copyright 2018 Elmish.XamarinForms contributors. See LICENSE.md for license.
namespace Demo.iOS

open System
open UIKit
open Foundation
open Xamarin.Forms
open Xamarin.Forms.Platform.iOS

module AppTypes = SuperClip.App.Types

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit FormsApplicationDelegate ()

    override this.FinishedLaunching (app, options) =
        Forms.Init()
        let appcore = new AppTypes.App()
        this.LoadApplication (appcore)
        base.FinishedLaunching(app, options)

module Main =
    [<EntryPoint>]
    let main args =
        UIApplication.Main(args, null, "AppDelegate")
        0

