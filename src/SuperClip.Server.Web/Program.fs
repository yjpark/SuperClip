module SuperClip.Server.Web.Program

open Saturn
open Giraffe

open Dap.Prelude
open Dap.Platform
open Dap.Remote.WebSocketHub
open SuperClip.Server

module CloudHubAgent = SuperClip.Server.CloudHub.Agent

[<EntryPoint>]
let main _ =
    let app = App.init "arbitrage-web-server-.log"

    let webRouter = router {
        not_found_handler (text "Api 404")
    }

    let webApp = application {
        url "http://0.0.0.0:5700/"
        use_router webRouter
        use_static "wwwroot"
        app_config (useWebSocketHub "/ws_user" app.Env CloudHubAgent.ServiceKind)
    }

    run webApp
    0