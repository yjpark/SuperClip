[<RequireQualifiedAccess>]
module SuperClip.Mac.Feature.LocalClipboard

open FSharp.Control.Tasks.V2
open AppKit
open Foundation

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.Core
open SuperClip.Core.Primary.Types
open SuperClip.Core.Primary.Tasks
module Base = SuperClip.Core.Feature.LocalClipboard

[<Literal>]
let NSStringPboardType = "NSStringPboardType"

let PboardTypes = [| "NSStringPboardType" |]

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    let pasteboard = NSPasteboard.GeneralPasteboard
    do (
        pasteboard.DeclareTypes (PboardTypes, null) |> ignore
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            let text = pasteboard.GetStringForType (NSStringPboardType);
            return Base.textToContent text
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            let text = Base.contentToText content
            pasteboard.SetStringForType (text, NSStringPboardType) |> ignore
            return ()
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        new Context (agent.Env.Logging) :> ILocalClipboard
