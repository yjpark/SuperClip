[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud.Leave

#if FABLE_COMPILER
module E = Thoth.Json.Encode
module D = Thoth.Json.Decode
#else
open Newtonsoft.Json
module E = Thoth.Json.Net.Encode
module D = Thoth.Json.Net.Decode
#endif

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

module Auth = SuperClip.Core.Cloud.Auth

type Req = Auth.Req

type Res = JsonBool

type Error = Auth.Error