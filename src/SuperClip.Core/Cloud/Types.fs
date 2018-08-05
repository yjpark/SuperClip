
module SuperClip.Core.Cloud.Types

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Req =
    | DoJoin of Join.Req
    | DoAuth of Auth.Req
    | DoLeave of Leave.Req
with
    static member JsonSpec =
        [
            CaseSpec<Req>.Create "DoJoin" [
                FieldSpec.Create<Join.Req> Join.Req.JsonEncoder Join.Req.JsonDecoder
            ]
            CaseSpec<Req>.Create "DoAuth" [
                FieldSpec.Create<Auth.Req> Auth.Req.JsonEncoder Auth.Req.JsonDecoder
            ]
            CaseSpec<Req>.Create "DoLeave" [
                FieldSpec.Create<Leave.Req> Leave.Req.JsonEncoder Leave.Req.JsonDecoder
            ]
        ]
    static member JsonDecoder = D.union Req.JsonSpec
    static member JsonEncoder = E.union Req.JsonSpec
    interface IJson with
        member this.ToJson () = Req.JsonEncoder this
    interface IRequest with
        member this.Kind = Union.getKind<Req> this

and ClientRes =
    | OnJoin of Join.Req * StubResult<Join.Res, Join.Error>
    | OnAuth of Auth.Req * StubResult<Auth.Res, Auth.Error>
    | OnLeave of Auth.Req * StubResult<Leave.Res, Leave.Error>
with
    static member StubSpec =
        [
            Stub.ResponseSpec<ClientRes>.Create "DoJoin" [
                FieldSpec.Create<Join.Req> Join.Req.JsonEncoder Join.Req.JsonDecoder
            ] "OnJoin" Join.Res.JsonDecoder Join.Error.JsonDecoder
            Stub.ResponseSpec<ClientRes>.Create "DoAuth" [
                FieldSpec.Create<Auth.Req> Auth.Req.JsonEncoder Auth.Req.JsonDecoder
            ] "OnAuth" Auth.Res.JsonDecoder Auth.Error.JsonDecoder
            Stub.ResponseSpec<ClientRes>.Create "DoLeave" [
                FieldSpec.Create<Leave.Req> Leave.Req.JsonEncoder Leave.Req.JsonDecoder
            ] "OnLeave" Leave.Res.JsonDecoder Leave.Error.JsonDecoder
        ]

type ServerReq =
    | DoJoin of Join.Req * Callback<Result<Join.Res, Join.Error>>
    | DoAuth of Auth.Req * Callback<Result<Auth.Res, Auth.Error>>
    | DoLeave of Leave.Req * Callback<Result<Leave.Res, Leave.Error>>
with
    static member HubSpec =
        [
            Hub.RequestSpec<ServerReq>.Create "DoJoin" [
                FieldSpec.Create<Join.Req> Join.Req.JsonEncoder Join.Req.JsonDecoder
            ] Hub.getCallback<Join.Res, Join.Error>
            Hub.RequestSpec<ServerReq>.Create "DoAuth" [
                FieldSpec.Create<Auth.Req> Auth.Req.JsonEncoder Auth.Req.JsonDecoder
            ] Hub.getCallback<Auth.Res, Auth.Error>
            Hub.RequestSpec<ServerReq>.Create "DoLeave" [
                FieldSpec.Create<Leave.Req> Leave.Req.JsonEncoder Leave.Req.JsonDecoder
            ] Hub.getCallback<Leave.Res, Leave.Error>
        ]
    interface IReq

and Evt =
    | OnJoin of Peer
    | OnLeave of Peer
    | OnChanged of Item
with
    static member JsonSpec =
        [
            CaseSpec<Evt>.Create "OnJoin" [
                FieldSpec.Create<Peer> Peer.JsonEncoder Peer.JsonDecoder
            ]
            CaseSpec<Evt>.Create "OnLeave" [
                FieldSpec.Create<Peer> Peer.JsonEncoder Peer.JsonDecoder
            ]
            CaseSpec<Evt>.Create "OnChanged" [
                FieldSpec.Create<Item> Item.JsonEncoder Item.JsonDecoder
            ]
        ]
    static member JsonDecoder = D.union Evt.JsonSpec
    static member JsonEncoder = E.union Evt.JsonSpec

    interface IJson with
        member this.ToJson () = Evt.JsonEncoder this
    interface IEvent with
        member this.Kind = Union.getKind<Evt> this

let StubSpec : StubSpec<Req, ClientRes, Evt> =
    {
        Response = ClientRes.StubSpec
        Event = Evt.JsonSpec
    }

let getHubSpec (getHub : GetHub<ServerReq, Evt>) = {
    Request = ServerReq.HubSpec
    GetHub = getHub
}
