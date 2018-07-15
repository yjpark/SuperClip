module SuperClip.Core.Primary.Tasks

open FSharp.Control.Tasks.V2
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.Primary.Types

let internal onGetFailed : OnFailed<Agent> =
    fun runner e ->
        runner.Deliver <| InternalEvt ^<| OnGet ^<| Error e

let internal doGetAsync : GetTask<Agent, unit> =
    fun runner -> task {
        let! text = CrossClipboard.Current.GetTextAsync ()
        let content = Clipboard.Text text
        runner.Deliver <| InternalEvt ^<| OnGet ^<| Ok content
    }

let internal onGetAsync
                (res : Result<Clipboard.Content, exn>)
                (current : Clipboard.Item option)
                (callbacks : (IReq * Callback<Item>) list)
                    : GetTask<Agent, unit> =
    fun runner -> task {
        res
        |> Result.iter (fun _ ->
            let current = current |> Option.get
            callbacks
            |> List.iterBack (fun (req, callback) ->
                reply runner callback <| ack req current
            )
            runner.Deliver <| Evt ^<| OnChanged current
        )
        res
        |> Result.iterError (fun e ->
            callbacks
            |> List.iterBack (fun (req, callback) ->
                reply runner callback <| nak req "Failed" e
            )
        )
    }