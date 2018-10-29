module SuperClip.Mac.Program

open System
open AppKit
open Eto.Forms
open Eto.Drawing

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto.Prefab

open SuperClip.Eto

[<EntryPoint>]
[<STAThread>]
let main args =
    let platform = new Eto.Mac.Platform ()
    let app = EtoApp.Create(platform)
    0 // return an integer exit code