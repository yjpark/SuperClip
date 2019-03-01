module SuperClip.macOS.Main
open System
open AppKit

[<STAThread>]
[<EntryPoint>]
let main args =
    NSApplication.Init ()
    NSApplication.SharedApplication.Delegate <- new AppDelegate();
    NSApplication.Main (args)
    0
