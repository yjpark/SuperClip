module SuperClip.Gtk.Program

open System
open System.Threading
open System.Threading.Tasks
open Eto.Forms
open Eto.Drawing

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto.Prefab

open SuperClip.Eto

[<EntryPoint>]
[<STAThread>]
let main argv =
    let platform = new Eto.GtkSharp.Platform ()
    let app = EtoApp.Create (platform = platform, consoleLogLevel = LogLevelWarning)
    0 // return an integer exit code