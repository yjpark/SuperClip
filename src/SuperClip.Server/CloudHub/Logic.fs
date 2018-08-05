[<RequireQualifiedAccess>]
module SuperClip.Server.CloudHub.Logic

open Elmish
open Dap.Prelude
open Dap.Platform

open SuperClip.Core
module CloudTypes = SuperClip.Core.Cloud.Types

open SuperClip.Server.CloudHub.Types
open SuperClip.Server.CloudHub.Tasks

type ActorOperate = ActorOperate<Agent, Args, Model, Msg, Req, Evt>
let private handleReq (req : Req) : ActorOperate =
    fun runner (model, cmd) ->
        (*
        match req with
        | CloudHubTypes.ServerReq.DoLogin (req', callback) ->
            replyAsync runner req callback nakOnFailed <| doLoginAsync req'
        | CloudHubTypes.ServerReq.DoAuth (token, callback) ->
            replyAsync runner req callback nakOnFailed <| doAuthAsync token
        | CloudHubTypes.ServerReq.DoRevoke (token, callback) ->
            replyAsync runner req callback nakOnFailed <| doRevokeAsync token
        | CloudHubTypes.ServerReq.ToUserEnv (req', callback) ->
            replyAsync runner req callback nakOnFailed <| toUserEnvAsync req'
        *)
        (model, cmd)

let private handleInternalEvt (evt : InternalEvt) : ActorOperate =
    fun runner (model, cmd) ->
        match evt with
        | OnDisconnected ->
            ()
            (*
            model.State.UserAuth <- None
            resetUser runner
            *)
        (model, cmd)

let private update : ActorUpdate<Agent, Args, Model, Msg, Req, Evt> =
    fun runner model msg ->
        match msg with
        | HubReq req ->
            handleReq req
        | HubEvt _evt ->
            noOperation
        | InternalEvt evt ->
            handleInternalEvt evt
        <| runner <| (model, [])

let private init : ActorInit<Args, Model, Msg> =
    fun _runner args ->
        let session = {
            Token = "TODO"
            (*
            Token = None
            UserAuth = None
            User = None
            UserEnv = None
            *)
        }
        ({
            Session = session
        }, Cmd.none)

let spec (args : Args) =
    new ActorSpec<Agent, Args, Model, Msg, Req, Evt>
        (Agent.Spawn, args, HubReq, castEvt, init, update)