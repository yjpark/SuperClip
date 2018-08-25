[<RequireQualifiedAccess>]
module SuperClip.Forms.Pref

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Credential = {
    Device : Device
    Channel : Channel
    PassHash : string
    CryptoKey : string
    Token : string
} with
    static member Create device channel passHash cryptoKey token = {
        Device = device
        Channel = channel
        PassHash = passHash
        CryptoKey = cryptoKey
        Token = token
    }
    static member JsonDecoder =
        D.decode Credential.Create
        |> D.required "device" Device.JsonDecoder
        |> D.required "channel" Channel.JsonDecoder
        |> D.required "pass_hash" D.string
        |> D.required "crypto_key" D.string
        |> D.optional "token" D.string ""
    static member JsonEncoder (this : Credential) =
        E.object [
            "device", Device.JsonEncoder this.Device
            "channel", Channel.JsonEncoder this.Channel
            "pass_hash", E.string this.PassHash
            "crypto_key", E.string this.CryptoKey
            "token", E.string this.Token
        ]
    interface IJson with
        member this.ToJson () =
            Credential.JsonEncoder this