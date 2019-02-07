module SuperClip.Web.Program

open System.Threading
open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Giraffe

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote.Web

open SuperClip.Server
module CloudHubTypes = SuperClip.Server.CloudHub.Types

open Dap.Remote.Dashboard

let router (app: IApp) =
    choose [
        GET >=>
            choose [
            ]
    ]

type WebApp (loggingArgs : LoggingArgs, args : AppArgs, port : int) =
    inherit App (loggingArgs, args)
    let web =
        new WebPack<IApp> (base.AsApp,
            WebHost.empty
            |> WebHost.setDevMode
            |> WebHost.setPort port
            |> WebHost.setStaticRoot "wwwroot"
            |> WebHost.setGiraffe (router base.AsApp)
            |> WebHost.setWebSocketHub (base.Env, "/ws_user", CloudHubTypes.GatewayKind)
            |> WebHost.setWebSocketHub (base.Env, "/ws_dashboard", Dap.Remote.Dashboard.OperatorHub.Types.GatewayKind)
        )
    member __.Web = web
    static member Run (port : int) =
        let (l, a) = App.CreateArgs("super-clip-web-.log")
        let app = new WebApp (l, a, port)
        app.Env.RunTask0 ignoreOnFailed (fun _ -> task{
            do! Task.Delay 5.0<second>
            app.Env.TakeSnapshot ()
            |> toJson
            |> E.encode 4
            |> logWip app.Env "Snapshot"
        })
        app.Web.Run ()
        0

[<EntryPoint>]
let main _ =
    WebApp.Run (5700)