[<AutoOpen>]
module SuperClip.Server.Types

open Farango.Types

open Dap.Prelude
open Dap.Platform

type App = {
    Env : IEnv
    Db : Connection
    Ident : string
} with
    member this.Log m = this.Env.Log m
    interface ILogger with
        member this.Log m = this.Env.Log m

type DbServiceArgs = {
    Db : Connection
} with
    static member Create conn =
        {
            Db = conn
        }