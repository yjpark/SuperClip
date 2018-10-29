[<AutoOpen>]
module SuperClip.App.Packs

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform
open Dap.Local
open SuperClip.Core

module SessionTypes = SuperClip.App.Session.Types

#if DAP_FORMS_FEATURE
type Preferences = Dap.Forms.Feature.Preferences.Context
type SecureStorage = Dap.Forms.Feature.SecureStorage.Context
#else
type Preferences = Dap.Local.Feature.Preferences.Context
type SecureStorage = Dap.Local.Feature.SecureStorage.Context
#endif

#if SUPERCLIP_CORE_FEATURE
type LocalClipboard = SuperClip.Core.Feature.LocalClipboard.Context
#endif
#if SUPERCLIP_FORMS_FEATURE
type LocalClipboard = SuperClip.Forms.Feature.LocalClipboard.Context
#endif
#if SUPERCLIP_MAC_FEATURE
type LocalClipboard = SuperClip.Mac.Feature.LocalClipboard.Context
#endif
#if SUPERCLIP_GTK_FEATURE
type LocalClipboard = SuperClip.Gtk.Feature.LocalClipboard.Context
#endif

(*
 * Generated: <Pack>
 *)
type ISessionPackArgs =
    inherit IClientPackArgs
    abstract Session : NoArgs with get
    abstract AsClientPackArgs : IClientPackArgs with get

type ISessionPack =
    inherit IPack
    inherit IClientPack
    abstract Args : ISessionPackArgs with get
    abstract Session : SessionTypes.Agent with get
    abstract AsClientPack : IClientPack with get