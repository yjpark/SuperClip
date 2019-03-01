module SuperClip.Ooui.Program

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Fabulous.Ooui

open SuperClip.App
open SuperClip.Fabulous

[<EntryPoint>]
[<STAThread>]
let main argv =
    setFabulousOouiParam <| OouiParam.Create ("SuperClip.Ooui", port = 6060)
    App.RunFabulous ("super-clip-.log")

