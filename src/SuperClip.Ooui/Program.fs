module SuperClip.Ooui.Program

open System
open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.App
open SuperClip.Forms

let onExit (app : IApp) (exited : AutoResetEvent) =
    fun (_sender : obj) (cancelArgs : ConsoleCancelEventArgs) ->
        logWip app "Quiting ..." cancelArgs
        app.Env.Logging.Close ()
        exited.Set() |> ignore

let waitForExit (app : IApp) =
    let exited = new AutoResetEvent(false)
    let onExit' = new ConsoleCancelEventHandler (onExit app exited)
    Console.CancelKeyPress.AddHandler onExit'
    exited.WaitOne() |> ignore

(*
let setTestDataAsync : GetTask<App.View, unit> =
    fun runner -> task {
        let primary = runner.ViewState.Parts.Primary
        for i in 1 .. 10 do
            do! Task.Delay 1.0<second>
            let text = sprintf "Test %A" i
            primary.Post <| Clipboard.DoSet (Text text) None
        return ()
    }
*)

let onAppStarted (app : FormsApp) =
    Xamarin.Forms.Forms.LoadApplication <| app.View.Application
    //app.View.RunTask ignoreOnFailed setTestDataAsync

[<EntryPoint>]
[<STAThread>]
let main argv =
    Ooui.UI.Port <- 6060
    Xamarin.Forms.Forms.Init ();
    let app = FormsApp.Run (onAppStarted)
    waitForExit app
    0 // return an integer exit code
