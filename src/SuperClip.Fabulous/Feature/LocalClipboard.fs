[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Feature.LocalClipboard

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Fabulous

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module Base = SuperClip.Core.Feature.LocalClipboard
type Provider = Xamarin.Essentials.Clipboard
type Fallback = SuperClip.Core.Feature.LocalClipboard.Context

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    let fallback : Fallback option =
        if hasEssentials () then
            None
        else
            Some <| new Fallback (logging)
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            match fallback with
            | Some fallback ->
                return! fallback.GetAsync.Handle ()
            | None ->
                let! text = Provider.GetTextAsync ()
                return Base.textToContent text
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            match fallback with
            | Some fallback ->
                return! fallback.SetAsync.Handle content
            | None ->
                do! Provider.SetTextAsync <| Base.contentToText content
                return ()
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)