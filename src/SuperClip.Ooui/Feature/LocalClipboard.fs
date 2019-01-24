[<RequireQualifiedAccess>]
module SuperClip.Ooui.Feature.LocalClipboard

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Forms
open Dap.Gui

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

type Context (logging : ILogging) =
    inherit SuperClip.Core.Feature.LocalClipboard.Context (logging)
    interface IOverride