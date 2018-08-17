module SuperClip.Web.Program

open Dap.Prelude
open Dap.Platform
module WebSocketHub = Dap.Remote.Web.WebSocketHub

module App = SuperClip.Server.App
module CloudHubAgent = SuperClip.Server.CloudHub.Agent

[<EntryPoint>]
let main _ =
    let app = App.create "super-clip-web-.log"
    app.Env |> WebSocketHub.run 5700 "/ws_user" CloudHubAgent.ServiceKind