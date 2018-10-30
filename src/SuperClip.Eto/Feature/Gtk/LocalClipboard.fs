[<RequireQualifiedAccess>]
module SuperClip.Gtk.Feature.LocalClipboard

#if SUPERCLIP_GTK_FEATURE
open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module Base = SuperClip.Core.Feature.LocalClipboard

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    let clipboardAtom = Gdk.Atom.Intern ("CLIPBOARD", false);
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            let clipboard = Gtk.Clipboard.Get (clipboardAtom)
            let text = clipboard.WaitForText ()
            return Base.textToContent text
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            let text = Base.contentToText content
            let clipboard = Gtk.Clipboard.Get(clipboardAtom)
            clipboard.Text <- text
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        new Context (agent.Env.Logging) :> ILocalClipboard
#endif
