[<AutoOpen>]
module SuperClip.Server.Types

open Farango.Types

open Dap.Prelude
open Dap.Platform
open Dap.Local.Farango.App

type App = WithDb.Model

let getInstance = WithDb.getInstance
