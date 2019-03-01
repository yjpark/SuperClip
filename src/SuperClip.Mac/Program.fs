module SuperClip.Mac.Program

open System
open AppKit

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Mac
open Dap.Fabulous.Mac

open SuperClip.App
open SuperClip.Gui
open SuperClip.Fabulous

let useFabulous = false

[<EntryPoint>]
[<STAThread>]
let main args =
    if useFabulous then
        setFabulousMacParam <| MacParam.Create ("SuperClip.Mac")
        App.RunFabulous ("super-clip-.log")
    else
        setMacParam <| MacParam.Create ("SuperClip.Mac")
        App.RunGui ("super-clip-.log")
