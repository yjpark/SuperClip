module SuperClip.Tools.Clipboard.WatchPrimary

open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Argu

open Dap.Prelude
open Dap.Platform

open SuperClip.Tools

type Args =
        | Duration of int
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Duration _ ->
                sprintf "Quit after <seconds>"

let executeAsync (app : IClientApp) (args : ParseResults<Args>) : Task<unit> = task {
    (* Todo
    app.AppState.Primary.Actor.OnEvent.AddWatcher app "Watch_Primary" (fun evt ->
        logWarn app "Watch_Primary" "On_Event" evt
    )
    *)
    ()
}
