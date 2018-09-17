﻿module SuperClip.Web.Program

open Dap.Prelude
open Dap.Platform
module WebSocketHub = Dap.Remote.Web.WebSocketHub

open SuperClip.Server.Helper
module CloudHubTypes = SuperClip.Server.CloudHub.Types

[<EntryPoint>]
let main _ =
    let app = createApp "super-clip-web-.log"
    app.Env |> WebSocketHub.run 5700 "/ws_user" CloudHubTypes.GatewayKind