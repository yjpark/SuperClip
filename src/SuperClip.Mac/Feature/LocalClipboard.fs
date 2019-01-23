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
    let mutable pasteboard : NSPasteboard option = None
    let getPasteboard () =
        if pasteboard.IsNone then
            pasteboard <- Some NSPasteboard.GeneralPasteboard
            pasteboard.Value.DeclareTypes (PboardTypes, null) |> ignore
        pasteboard.Value
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupGuiHandler (fun () -> task {
            let text = (getPasteboard ()).GetStringForType (NSStringPboardType);
            return Base.textToContent text
        })
        base.SetAsync.SetupGuiHandler (fun (content : Content) -> task {
            let text = Base.contentToText content
            (getPasteboard ()).SetStringForType (text, NSStringPboardType) |> ignore
            return ()
        })
    )
    interface IOverride
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        new Context (agent.Env.Logging) :> ILocalClipboard
