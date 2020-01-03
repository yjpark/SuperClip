module SuperClip.Uno.Program

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Fabulous.Uno

open SuperClip.App
open SuperClip.Fabulous

[<EntryPoint>]
[<STAThread>]
let main argv =
    setFabulousUnoParam <| UnoParam.Create ("SuperClip.Uno", port = 6060)
    App.RunFabulous ("super-clip-.log")

