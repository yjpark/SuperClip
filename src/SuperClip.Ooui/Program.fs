module SuperClip.Program

open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Plugin.Clipboard

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.Clipboard
module App = SuperClip.Forms.App

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

let setTestDataAsync : GetTask<App.View, unit> =
    fun runner -> task {
        let primary = runner.ViewState.Primary
        for i in 1 .. 10 do
            do! Task.Delay 1.0<second>
            let text = sprintf "Test %A" i
            primary.Post <| Clipboard.DoSet (Text text) None
        return ()
    }

let onAppStarted (app : App.Model) =
    Xamarin.Forms.Forms.LoadApplication <| app.View.Application
    app.View.RunTask ignoreOnFailed setTestDataAsync

[<EntryPoint>]
let main argv =
    Ooui.UI.Port <- 6060
    Xamarin.Forms.Forms.Init ();
    CrossClipboard.Current <- new ClipboardImplementation ()
    let app = App.create onAppStarted
    (*
    App.initApplication () |> ignore
    let env = App.getEnv ()
    (App.getOnInit ()) .AddWatcher env "Ooui_Init" (fun _ ->
        Xamarin.Forms.Forms.LoadApplication <| App.getApplication ()
        let app = App.getApp ()
        app.RunTask ignoreOnFailed setTestDataAsync
    )
    *)
    waitForExit app.Env
    0 // return an integer exit code
