module SuperClip.Web.Program

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Giraffe

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote.Web
open Dap.Remote.Web.App

open SuperClip.Server
module CloudHubTypes = SuperClip.Server.CloudHub.Types

open Dap.Remote.Dashboard

let router (app : IApp) =
    choose [
        GET >=>
            choose [
            ]
    ]

let newWebHost (port : int) (app : IApp) =
    app.Env.RunTask0 ignoreOnFailed (fun _ -> task{
        do! Task.Delay 5.0<second>
        app.Env.TakeSnapshot ()
        |> toJson
        |> E.encode 4
        |> logWip app.Env "Snapshot"
    })
    WebHost.empty
    |> WebHost.setDevMode
    |> WebHost.setPort port
    |> WebHost.setStaticRoot "wwwroot"
    |> WebHost.setGiraffe (router app)
    |> WebHost.setWebSocketHub (app.Env, "/ws_user", CloudHubTypes.GatewayKind)
    |> WebHost.setWebSocketHub (app.Env, "/ws_dashboard", Dap.Remote.Dashboard.OperatorHub.Types.GatewayKind)

type App with
    static member RunWeb (port, logFile, ?scope : string, ?consoleMinLevel : LogLevel) : int =
        let app = App.Create (logFile, ?scope = scope, ?consoleMinLevel = consoleMinLevel)
        app.Start ()
        app.AsApp |> runWebApp<IApp> (newWebHost port) (fun () ->
            ()
        )

[<EntryPoint>]
let main _ =
    App.RunWeb (5700, "super-clip-web-.log")
