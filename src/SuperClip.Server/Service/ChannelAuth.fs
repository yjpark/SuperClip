[<RequireQualifiedAccess>]
module SuperClip.Server.Service.ChannelAuth

open System.Threading.Tasks
open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
open Dap.Local.Farango
open Dap.Remote.Server.Auth

open SuperClip.Core

module Document = Dap.Local.Farango.Util.Document

[<Literal>]
let Collection = "channel_auth"

type Record = {
    Channel : Channel
    PassHash : string
    Tokens : Token list
} with
    static member Create channel passHash tokens = {
        Channel = channel
        PassHash = passHash
        Tokens = tokens
    }
    static member JsonDecoder =
        D.object (fun get ->
            {
                Channel = get.Required.Field "channel" Channel.JsonDecoder
                PassHash =  get.Required.Field "pass_hash" D.string
                Tokens = get.Optional.Field "tokens" Tokens.JsonDecoder
                    |> Option.defaultValue []
            }
        )
    static member JsonEncoder (this : Record) =
        E.object [
            "_key", E.string this.Key
            "channel", Channel.JsonEncoder this.Channel
            "pass_hash", E.string this.PassHash
            "tokens", Tokens.JsonEncoder true this.Tokens
        ]
    interface IJson with
        member this.ToJson () = Record.JsonEncoder this
    member this.Key = this.Channel.Guid
    member this.WithTokens tokens = {this with Tokens = tokens}

let getByChannelKeyAsync (key : string) (db : IDbPack) : Task<Result<Record, string>> = task {
    return! db.Db |> Document.getAsync Collection key Record.JsonDecoder
}

let createAsync (record : Record) (db : IDbPack) = task {
    return! db.Db |> Document.createAsync Collection record.Channel.Key (encodeJson 4 record)
}

let addTokenAsync token (record : Record) app = Tokens.addTokenAsync Collection token record app
let removeTokenAsync token (record : Record) app = Tokens.removeTokenAsync Collection token record app
let checkToken token (record : Record) app = Tokens.checkToken token record app
