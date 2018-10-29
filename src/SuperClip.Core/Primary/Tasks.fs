module SuperClip.Core.Primary.Tasks

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Core.Primary.Types

let internal onGetFailed : OnFailed<Agent> =
    fun runner e ->
        runner.Deliver <| InternalEvt ^<| OnGet ^<| Error e

let internal doGetAsync (index : int32) : GetTask<Agent, unit> =
    fun runner -> task {
        let! content = runner.Pack.LocalClipboard.Context.GetAsync.Handle ()
        if runner.Actor.State.GettingIndex = index then
            runner.Deliver <| InternalEvt ^<| OnGet ^<| Ok content
        else
            logWarn runner "doGetAsync" "Timeout" (index, runner.Actor.State.GettingIndex, content)
    }

let internal doSetAsync (content : Content) : GetTask<Agent, unit> =
    fun runner -> task {
        do! runner.Pack.LocalClipboard.Context.SetAsync.Handle content
    }

let internal onGetAsync
                (res : Result<Content, exn>)
                (current : Item)
                (callbacks : (IReq * Callback<Item>) list)
                    : GetTask<Agent, unit> =
    fun runner -> task {
        res
        |> Result.iter (fun _ ->
            callbacks
            |> List.iterBack (fun (req, callback) ->
                reply runner callback <| ack req current
            )
        )
        res
        |> Result.iterError (fun e ->
            callbacks
            |> List.iterBack (fun (req, callback) ->
                reply runner callback <| nak req "Failed" e
            )
        )
    }