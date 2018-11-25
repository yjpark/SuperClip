[<RequireQualifiedAccess>]
module SuperClip.Core.Feature.LocalClipboard

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local

open SuperClip.Core
open System.Runtime.InteropServices

[<Literal>]
let AssetPrefix = "__SUPERCLIP_ASSET__"

[<Literal>]
let UseShellClipboard = true

let mutable private text' = ""
let getText () =
    if UseShellClipboard then
        if RuntimeInformation.IsOSPlatform (OSPlatform.OSX) then
            Shell.bash "pbpaste"
        elif RuntimeInformation.IsOSPlatform (OSPlatform.Linux) then
            Shell.bash "xsel -b"
        else
            text'
    else
        text'

let setText text =
    text' <- text
    if UseShellClipboard then
        let text = text |> Shell.escape
        if RuntimeInformation.IsOSPlatform (OSPlatform.OSX) then
            Shell.bash <| sprintf "echo \"%s\" | pbcopy" text
        elif RuntimeInformation.IsOSPlatform (OSPlatform.Linux) then
            Shell.bash <| sprintf "echo \"%s\" | xsel -i -b" text
        elif RuntimeInformation.IsOSPlatform (OSPlatform.Windows) then
            Shell.bat <| sprintf "echo %s | clip" text
        else
            ""
        |> ignore

let textToContent (text : string) : Content =
    if text.StartsWith AssetPrefix then
        Content.CreateAsset <| text.Replace (AssetPrefix, "")
    else
        Content.CreateText <| text

let contentToText (content : Content) : string =
    match content with
    | NoContent -> ""
    | Text text -> text
    | Asset asset -> sprintf "%s%s" AssetPrefix asset

type Context (logging : ILogging) =
    inherit BaseLocalClipboard<Context> (logging)
    do (
        base.SetSupportOnChanged false
        base.GetAsync.SetupHandler (fun () -> task {
            return textToContent <| getText ()
        })
        base.SetAsync.SetupHandler (fun (content : Content) -> task {
            setText <| contentToText content
            return ()
        })
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        new Context (agent.Env.Logging) :> ILocalClipboard

    interface IFallback
