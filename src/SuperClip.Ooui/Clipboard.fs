module SuperClip.Clipboard

open System;
open System.Threading.Tasks;
open FSharp.Control.Tasks.V2

open NodaTime
open Plugin.Clipboard.Abstractions;

open Dap.Platform
open Dap.Local

[<Literal>]
let UseShellClipboard = false

type ClipboardImplementation () =
    let mutable text' = ""
    member _this.GetText () =
        if UseShellClipboard then
            match Runtime.Platform with
            | Mac ->
                Shell.bash "pbpaste"
            | Linux ->
                Shell.bash "xsel -b"
            | Windows ->
                text'
        else
            text'
    member _this.SetText text =
        text' <- text
        let text = text |> Shell.escape
        match Runtime.Platform with
        | Mac ->
            Shell.bash <| sprintf "echo \"%s\" | pbcopy" text
        | Linux ->
            Shell.bash <| sprintf "echo \"%s\" | xsel -i -b" text
        | Windows ->
            Shell.bat <| sprintf "echo %s | clip" text
        |> ignore
    interface IClipboard with
        member this.GetTextAsync () = task {
            return this.GetText ()
        }
        member this.GetText () = this.GetText ()
        member this.SetText text = this.SetText text