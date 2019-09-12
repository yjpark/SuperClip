[<AutoOpen>]
module SuperClip.Server.Helper

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local
open Dap.Local.Farango

[<Literal>]
let Scope = "SuperClipServer"

[<Literal>]
let DbConfigLuid = "db_config.json"

let DbUri =
    IEnvironment.Instance.TryGetVariable "SUPERCLIP_DB_URL"
    |> Option.defaultValue "http://superclip_dev:VauCha0jeeph2jie2eeriesh@localhost:11001/superclip_dev"

type App with
    static member Create (logFile, ?scope : string, ?consoleMinLevel : LogLevel) =
        let scope = defaultArg scope Scope
        let loggingArgs = LoggingArgs.CreateBoth (logFile, ?consoleMinLevel = consoleMinLevel)
        let args =
            AppArgs.Create ()
            |> fun a -> a.WithScope scope
            |> (
                IEnvironment.Instance.Preferences.Get.Handle DbConfigLuid
                |> Option.get
                |> decodeJson DbArgs.JsonDecoder
                |> AppArgs.SetFarangoDb
            )
        new App (loggingArgs, args)

