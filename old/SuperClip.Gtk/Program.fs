module SuperClip.Gtk.Program

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto.Prefab
open Dap.Eto.Gtk

open SuperClip.Eto

[<EntryPoint>]
[<STAThread>]
let main argv =
    EtoApp.Run (etoPlatform ())