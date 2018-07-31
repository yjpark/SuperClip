module SuperClip.Program

open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Xamarin.Forms
open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.App.Types
open SuperClip.Clipboard
module App = SuperClip.App.App

let onExit (env : IEnv) (exited : AutoResetEvent) =
    fun (_sender : obj) (cancelArgs : ConsoleCancelEventArgs) ->
        logWip env "Quiting ..." cancelArgs
        env.Logging.Close ()
        exited.Set() |> ignore

let waitForExit (env : IEnv) =
    let exited = new AutoResetEvent(false)
    let onExit' = new ConsoleCancelEventHandler (onExit env exited)
    Console.CancelKeyPress.AddHandler onExit'
    exited.WaitOne() |> ignore

let setTestDataAsync : GetTask<App, unit> =
    fun runner -> task {
        let primary = runner.AppState.Primary
        for i in 1 .. 10 do
            do! Task.Delay 1.0<second>
            let text = sprintf "Test %A" i
            primary.Post <| Clipboard.DoSet' (Clipboard.Text text) None
        return ()
    }

[<EntryPoint>]
let main argv =
    Xamarin.Forms.Forms.Init ();
    CrossClipboard.Current <- new ClipboardImplementation ()
    App.initApplication () |> ignore
    let env = App.getEnv ()
    (App.getOnInit ()) .AddWatcher env "Ooui_Init" (fun _ ->
        Xamarin.Forms.Forms.LoadApplication <| App.getApplication ()
        let app = App.getApp ()
        app.RunTask ignoreOnFailed setTestDataAsync
    )
    waitForExit env
    0 // return an integer exit code
