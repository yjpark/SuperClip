[<AutoOpen>]
module SuperClip.Core.Const

open Dap.Prelude
open Dap.Platform

[<Literal>]
let PassHashSalt = "ahDaefam2DecaR0ooyak6ohghah0coh6"

[<Literal>]
let CryptoKeySalt = "shoo4ree3daiH9go4Yeir8rah4ua7fei"

[<Literal>]
//let CloudServerUri = "ws://localhost:5700/ws_user"
let CloudServerUri = "ws://yjpark.org:5700/ws_user"

let calcPassHash (password : string) =
    calcSha256SumWithSalt PassHashSalt password

let calcCryptoKey (password : string) =
    calcSha256SumWithSalt CryptoKeySalt password
