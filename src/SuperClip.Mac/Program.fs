module SuperClip.Mac.Program

open System
open AppKit

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto.Prefab
open Dap.Eto.Mac

open SuperClip.Eto

[<EntryPoint>]
[<STAThread>]
let main args =
    EtoApp.Run (etoPlatform (new Eto.Mac.Platform ()))