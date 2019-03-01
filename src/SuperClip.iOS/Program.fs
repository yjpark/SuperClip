module SuperClip.iOS.Program

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.iOS
open Dap.Fabulous.iOS

open SuperClip.App
open SuperClip.Fabulous

[<EntryPoint>]
[<STAThread>]
let main args =
    setFabulousIOSParam <| IOSParam.Create ("SuperClip.iOS")
    App.RunFabulous ("super-clip-.log")