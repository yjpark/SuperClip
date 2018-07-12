namespace SuperClip.macOS
open System
open Foundation
open AppKit

open Xamarin.Forms
open Xamarin.Forms.Platform.MacOS

module AppTypes = SuperClip.App.Types

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit FormsApplicationDelegate ()

    let style = NSWindowStyle.Closable ||| NSWindowStyle.Resizable ||| NSWindowStyle.Titled
    let rect = new CoreGraphics.CGRect (200.0, 100.0, 1024.0, 768.0)
    let window = new NSWindow (rect, style, NSBackingStore.Buffered, false)
    do (
        window.Title <- "Xamarin.Forms on Mac!"
        //window.TitleVisibility <- NSWindowTitleVisibility.Hidden
    )
    override this.MainWindow = window
    override this.DidFinishLaunching (notification : NSNotification) =
        Forms.Init ()
        this.LoadApplication (new AppTypes.App())
        base.DidFinishLaunching(notification)