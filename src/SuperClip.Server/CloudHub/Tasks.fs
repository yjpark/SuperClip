module SuperClip.Server.CloudHub.Tasks

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
open Dap.Remote.Server.Auth

open SuperClip.Core
open SuperClip.Core.Cloud
open SuperClip.Server
open SuperClip.Server.CloudHub.Types

module ChannelAuth = SuperClip.Server.Service.ChannelAuth
module ChannelTypes = SuperClip.Core.Channel.Types
module ChannelService = SuperClip.Core.Channel.Service

let getTokenAndChannelAuthAsync (token : string) : GetTask<Agent, Result<Token * Device * ChannelAuth.Record, string>> =
    fun runner -> task {
        return!
            token
            |> Token.decodeJwt runner
            |> Result.bind ^<| Token.check runner
            |> Result.bind (fun token ->
                token.Data
                |> tryCastJson Device.JsonDecoder
                |> Result.map (fun device -> (token, device))
            )|> Result.toAsync
            |> Result.bindAsync (fun (token, device) ->
                try
                    (runner.Actor.Args |> ChannelAuth.getByChannelKeyAsync token.OwnerKey)
                    |> Result.mapAsync (fun auth -> task {
                        return (token, device, auth)
                    })
                with e -> task {
                    logException runner "getChannelAuthAsync" "Exception_Raised" (token, callback) e
                    return Error e.Message
                }
            )
    }

let private createToken runner (channelKey : string) (req : Join.Req) =
    let token = Token.create runner channelKey "" (E.json req.Peer.Device) (Duration.FromDays(1))
    let jwt = JsonString <| Token.toJwt token
    (token, jwt)
    //reply runner callback <| ack req ^<| Ok jwt

let doJoinAsync (join : Join.Req) : GetReplyTask<Agent, Result<Join.Res, Join.Err>> =
    fun req callback runner -> task {
        let app = runner.Actor.Args
        let! auth = runner.Actor.Args |> ChannelAuth.getByChannelKeyAsync join.Peer.Channel.Key
        match auth with
        | Ok auth ->
            if join.PassHash = auth.PassHash then
                let (token, jwt) = createToken runner auth.Channel.Key join
                let! result = app |> ChannelAuth.addTokenAsync token auth
                match result with
                | Ok _ ->
                    reply runner callback <| ack req ^<| Ok jwt
                | Error err ->
                    logWarn runner "doJoinAsync" "AddToken_Failed" (token, auth)
                    reply runner callback <| ack req ^<| Error Join.JoinChannelFailed
            else
                logWarn runner "doJoinAsync" "PassHashNotMatched" (join, auth.PassHash)
                reply runner callback <| ack req ^<| Error Join.PassHashNotMatched
        | Error err ->
            logWarn runner "doJoinAsync" "getByChannelKeyAsync" (join, err)
            if join.Peer.Channel.Name = "" then
                reply runner callback <| ack req ^<| Error Join.InvalidChannelName
            else
                let channel = Channel.CreateWithName join.Peer.Channel.Name
                let (token, jwt) = createToken runner channel.Key join
                let auth = ChannelAuth.Record.Create channel join.PassHash [token]
                let! result = app |> ChannelAuth.createAsync auth
                match result with
                | Ok _ ->
                    reply runner callback <| ack req ^<| Ok jwt
                | Error err ->
                    logWarn runner "doJoinAsync" "Create_ChannelAuth_Failed" (token, E.encodeJson 4 auth, err)
                    reply runner callback <| ack req ^<| Error Join.JoinChannelFailed
    }

let getOrAddChannelAsync (channel : Channel) (device : Device) : GetTask<Agent, ChannelService.Service> =
    fun runner -> task {
        let service =
            runner.Actor.State.Devices
            |> Map.tryFind channel.Key
        match service with
        | Some (service, existDevice) ->
            if device.Guid <> existDevice.Guid then
                runner.Deliver <| InternalEvt ^<| AddDevice (service, device)
            return service
        | None ->
            let! service = runner |> setupChannelServiceAsync channel
            runner.Deliver <| InternalEvt ^<| AddDevice (service, device)
            return service
    }

let doAuthAsync (auth : Auth.Req) : GetReplyTask<Agent, Result<Auth.Res, Auth.Err>> =
    fun req callback runner -> task {
        let! result = runner |> getTokenAndChannelAuthAsync auth.Value
        match result with
        | Ok (token, device, auth) ->
            let app = runner.Actor.Args
            match app |> ChannelAuth.checkToken token auth with
            | Ok (auth, token) ->
                let! channel = runner |> getOrAddChannelAsync auth.Channel device
                let peers = Peers.Create auth.Channel channel.Actor.State.Devices
                reply runner callback <| ack req ^<| Ok peers
            | Error err ->
                logWarn runner "doAuthAsync" "CheckToken_Failed" (token, auth)
                reply runner callback <| ack req ^<| Error Auth.InvalidToken
        | Error err ->
            reply runner callback <| ack req ^<| Error Auth.InvalidToken
    }

let doLeaveAsync (auth : Leave.Req) : GetReplyTask<Agent, Result<Leave.Res, Leave.Err>> =
    fun req callback runner -> task {
        let! result = runner |> getTokenAndChannelAuthAsync auth.Value
        match result with
        | Ok (token, device, auth) ->
            runner.Deliver <| InternalEvt ^<| RemoveDevice (auth.Channel.Key, device)
            let app = runner.Actor.Args
            let! result = app |> ChannelAuth.removeTokenAsync token auth
            match result with
            | Ok (_auth, removed) ->
                reply runner callback <| ack req ^<| Ok ^<| JsonBool removed
            | Error err ->
                logWarn runner "doLeaveAsync" "RemoveToken_Failed" (token, auth)
                failWith "RemoveToken_Failed" err
        | Error err ->
            reply runner callback <| ack req ^<| Error Auth.InvalidToken
    }
