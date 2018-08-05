[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Tools.Util

open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Argu
open Dap.Prelude
open Dap.Platform

open SuperClip.Forms.Types
open SuperClip.Tools

let onExit (app : App) (exited : AutoResetEvent) =
    fun (_sender : obj) (cancelArgs : ConsoleCancelEventArgs) ->
        logWip app.Env "Quiting ..." cancelArgs
        app.Env.Logging.Close ()
        exited.Set() |> ignore

let waitForExit (app : App) =
    let exited = new AutoResetEvent(false)
    let onExit' = new ConsoleCancelEventHandler (onExit app exited)
    Console.CancelKeyPress.AddHandler onExit'
    exited.WaitOne() |> ignore

let executeAndWaitForExit (app : App) (task : Task<unit>) =
    try
        Async.AwaitTask task
        |> Async.RunSynchronously
        waitForExit app
    with e ->
        logError app.Env "Execute" "Exception_Raised" (task.ToString (), e)