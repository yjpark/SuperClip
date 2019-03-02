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
    let fallback = new Fallback (logging)
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            if hasEssentials () then
                let! text = Provider.GetTextAsync ()
                return Base.textToContent text
            else
                return! fallback.GetAsync.Handle ()
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            if hasEssentials () then
                do! Provider.SetTextAsync <| Base.contentToText content
                return ()
            else
                return! fallback.SetAsync.Handle content
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)