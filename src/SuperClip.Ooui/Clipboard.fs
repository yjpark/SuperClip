module SuperClip.Clipboard

open System;
open System.Threading.Tasks;
open FSharp.Control.Tasks.V2

open NodaTime
open Plugin.Clipboard.Abstractions;

open Dap.Platform

type ClipboardImplementation () =
    let mutable text' = ""
    member _this.GetText () =
        text'
    member _this.SetText text =
        text' <- text
    interface IClipboard with
        member this.GetTextAsync () = task {
            return this.GetText ()
        }
        member this.GetText () = this.GetText ()
        member this.SetText text = this.SetText text