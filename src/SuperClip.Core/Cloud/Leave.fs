[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Leave

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

module Auth = SuperClip.Core.Cloud.Auth

type Req = Auth.Req

type Res = JsonBool

type Err = Auth.Err