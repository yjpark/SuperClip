[<RequireQualifiedAccess>]
module SuperClip.Wpf.Feature.LocalClipboard

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module Base = SuperClip.Core.Feature.LocalClipboard
type Provider = System.Windows.Clipboard

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    do (
        let owner = base.AsOwner
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            let text = Provider.GetText ()
            return Base.textToContent text
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            Provider.SetText <| Base.contentToText content
            return ()
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
