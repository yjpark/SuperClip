[<RequireQualifiedAccess>]
module SuperClip.Tools.Misc.InitDb

open Argu
open Farango.Collections

open Dap.Prelude
open Dap.Platform
open Dap.Local.Farango
open Dap.Local.Farango.Util

open SuperClip.Tools

let Collections = [
    CollectionDef.Create "channel_auth" [
        FullTextIndex (["channel.name"], 3)
    ]
]

type Args =
    | Force
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Force -> "Drop Collections if exist"

let execute (app : App) (args : ParseResults<Args>) =
    let force = args.Contains Force
    app.Db |> InitDb.createCollectionsAsync' force Collections
    |> Async.RunSynchronously
    |> ignore
