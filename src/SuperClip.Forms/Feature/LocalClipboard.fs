[<RequireQualifiedAccess>]
module SuperClip.Forms.Feature.LocalClipboard

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Forms

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks

module Base = SuperClip.Core.Feature.LocalClipboard
type Provider = Xamarin.Essentials.Clipboard
type Fallback = SuperClip.Core.Feature.LocalClipboard.Context

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupHandler (fun () -> task {
            //TODO: RUN IN UI THREAD
            let! text = Provider.GetTextAsync ()
            return Base.textToContent text
        })
        base.SetAsync.SetupHandler (fun (content : Content) -> task {
            Provider.SetText <| Base.contentToText content
            return ()
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        if hasEssentials () then
            new Context (agent.Env.Logging) :> ILocalClipboard
        else
            Fallback.AddToAgent agent