[<AutoOpen>]
module SuperClip.Core.Const

open Dap.Prelude
open Dap.Platform

[<Literal>]
let PassHashSalt = "ahDaefam2DecaR0ooyak6ohghah0coh6"

[<Literal>]
let CryptoKeySalt = "shoo4ree3daiH9go4Yeir8rah4ua7fei"

[<Literal>]
//let DefaultCloudServerUri = "ws://localhost:5700/ws_user"
let DefaultCloudServerUri = "wss://superclip.yjpark.org/ws_user"

let getDefaultCloudServerUri () = DefaultCloudServerUri

let verifyCloudServerUri (uri : string) =
    //Do verification
    uri

let calcPassHash (password : string) =
    Sha256.ofText2 PassHashSalt password

let calcCryptoKey (password : string) =
    Sha256.ofText2 CryptoKeySalt password
