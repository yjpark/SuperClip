module SuperClip.Wpf.Program

open System
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
    EtoApp.Run (platform = new Eto.Wpf.Platform ())