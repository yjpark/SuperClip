module SuperClip.Web.Program

open Giraffe

open Dap.Prelude
open Dap.Platform
open Dap.Remote.Web

open SuperClip.Server
module CloudHubTypes = SuperClip.Server.CloudHub.Types

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
        )
    member __.Web = web
    static member Run (port : int) =
        let (l, a) = App.CreateArgs("super-clip-web-.log")
        let app = new WebApp (l, a, port)
        app.Web.Run ()
        0

[<EntryPoint>]
let main _ =
    WebApp.Run (5700)