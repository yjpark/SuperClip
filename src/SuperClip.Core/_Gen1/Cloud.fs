[<RequireQualifiedAccess>]
module SuperClip.Core.Cloud

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

[<StructuredFormatDisplay("{AsDisplay}")>]
(*
 * Generated: <Record>
 *     IsJson
 *)
type JoinReq = {
    Peer : (* JoinReq *) Peer
    PassHash : (* JoinReq *) string
} with
    static member Create
        (
            ?peer : (* JoinReq *) Peer,
            ?passHash : (* JoinReq *) string
        ) : JoinReq =
        {
            Peer = (* JoinReq *) peer
                |> Option.defaultWith (fun () -> (Peer.Create ()))
            PassHash = (* JoinReq *) passHash
                |> Option.defaultWith (fun () -> "")
        }
    static member SetPeer ((* JoinReq *) peer : Peer) (this : JoinReq) =
        {this with Peer = peer}
    static member SetPassHash ((* JoinReq *) passHash : string) (this : JoinReq) =
        {this with PassHash = passHash}
    static member JsonEncoder : JsonEncoder<JoinReq> =
        fun (this : JoinReq) ->
            E.object [
                "peer", Peer.JsonEncoder (* JoinReq *) this.Peer
                "pass_hash", E.string (* JoinReq *) this.PassHash
            ]
    static member JsonDecoder : JsonDecoder<JoinReq> =
        D.object (fun get ->
            {
                Peer = get.Required.Field (* JoinReq *) "peer" Peer.JsonDecoder
                PassHash = get.Required.Field (* JoinReq *) "pass_hash" D.string
            }
        )
    static member JsonSpec =
        FieldSpec.Create<JoinReq> (JoinReq.JsonEncoder, JoinReq.JsonDecoder)
    interface IJson with
        member this.ToJson () = JoinReq.JsonEncoder this
    interface IObj
    member this.WithPeer ((* JoinReq *) peer : Peer) =
        this |> JoinReq.SetPeer peer
    member this.WithPassHash ((* JoinReq *) passHash : string) =
        this |> JoinReq.SetPassHash passHash
    member this.AsDisplay =
        (this.Peer, (String.capped 12 this.PassHash))

type JoinRes = JsonString

(*
 * Generated: <Union>
 *     IsJson
 *)
type JoinErr =
    | PassHashNotMatched
    | InvalidChannelName
    | JoinChannelFailed
with
    static member CreatePassHashNotMatched () : JoinErr =
        PassHashNotMatched
    static member CreateInvalidChannelName () : JoinErr =
        InvalidChannelName
    static member CreateJoinChannelFailed () : JoinErr =
        JoinChannelFailed
    static member JsonSpec' : CaseSpec<JoinErr> list =
        [
            CaseSpec<JoinErr>.Create ("PassHashNotMatched", [])
            CaseSpec<JoinErr>.Create ("InvalidChannelName", [])
            CaseSpec<JoinErr>.Create ("JoinChannelFailed", [])
        ]
    static member JsonEncoder = E.union JoinErr.JsonSpec'
    static member JsonDecoder = D.union JoinErr.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<JoinErr> (JoinErr.JsonEncoder, JoinErr.JsonDecoder)
    interface IJson with
        member this.ToJson () = JoinErr.JsonEncoder this

type AuthReq = JsonString

type AuthRes = Peers

(*
 * Generated: <Union>
 *     IsJson
 *)
type AuthErr =
    | InvalidToken
with
    static member Create () : AuthErr =
        InvalidToken
    static member JsonSpec' : CaseSpec<AuthErr> list =
        [
            CaseSpec<AuthErr>.Create ("InvalidToken", [])
        ]
    static member JsonEncoder = E.union AuthErr.JsonSpec'
    static member JsonDecoder = D.union AuthErr.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<AuthErr> (AuthErr.JsonEncoder, AuthErr.JsonDecoder)
    interface IJson with
        member this.ToJson () = AuthErr.JsonEncoder this

type LeaveReq = AuthReq

type LeaveRes = JsonBool

type LeaveErr = AuthErr

type SetItemReq = Item

type SetItemRes = JsonNil

(*
 * Generated: <Union>
 *     IsJson
 *)
type SetItemErr =
    | InvalidSource
    | InvalidChannel
with
    static member CreateInvalidSource () : SetItemErr =
        InvalidSource
    static member CreateInvalidChannel () : SetItemErr =
        InvalidChannel
    static member JsonSpec' : CaseSpec<SetItemErr> list =
        [
            CaseSpec<SetItemErr>.Create ("InvalidSource", [])
            CaseSpec<SetItemErr>.Create ("InvalidChannel", [])
        ]
    static member JsonEncoder = E.union SetItemErr.JsonSpec'
    static member JsonDecoder = D.union SetItemErr.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<SetItemErr> (SetItemErr.JsonEncoder, SetItemErr.JsonDecoder)
    interface IJson with
        member this.ToJson () = SetItemErr.JsonEncoder this

(*
 * Generated: <Union>
 *     IsJson
 *)
type Evt =
    | OnPeerJoin of peer : Peer
    | OnPeerLeft of peer : Peer
    | OnItemChanged of item : Item
with
    static member CreateOnPeerJoin peer : Evt =
        OnPeerJoin (peer)
    static member CreateOnPeerLeft peer : Evt =
        OnPeerLeft (peer)
    static member CreateOnItemChanged item : Evt =
        OnItemChanged (item)
    static member JsonSpec' : CaseSpec<Evt> list =
        [
            CaseSpec<Evt>.Create ("OnPeerJoin", [
                Peer.JsonSpec
            ])
            CaseSpec<Evt>.Create ("OnPeerLeft", [
                Peer.JsonSpec
            ])
            CaseSpec<Evt>.Create ("OnItemChanged", [
                Item.JsonSpec
            ])
        ]
    static member JsonEncoder = E.union Evt.JsonSpec'
    static member JsonDecoder = D.union Evt.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Evt> (Evt.JsonEncoder, Evt.JsonDecoder)
    interface IJson with
        member this.ToJson () = Evt.JsonEncoder this
    interface IEvent with
        member this.Kind = Union.getKind<Evt> this

(*
 * Generated: <Union>
 *     IsJson
 *)
type Req =
    | DoJoin of req : JoinReq
    | DoAuth of req : AuthReq
    | DoLeave of req : LeaveReq
    | DoSetItem of req : SetItemReq
with
    static member CreateDoJoin req : Req =
        DoJoin (req)
    static member CreateDoAuth req : Req =
        DoAuth (req)
    static member CreateDoLeave req : Req =
        DoLeave (req)
    static member CreateDoSetItem req : Req =
        DoSetItem (req)
    static member JsonSpec' : CaseSpec<Req> list =
        [
            CaseSpec<Req>.Create ("DoJoin", [
                JoinReq.JsonSpec
            ])
            CaseSpec<Req>.Create ("DoAuth", [
                AuthReq.JsonSpec
            ])
            CaseSpec<Req>.Create ("DoLeave", [
                LeaveReq.JsonSpec
            ])
            CaseSpec<Req>.Create ("DoSetItem", [
                SetItemReq.JsonSpec
            ])
        ]
    static member JsonEncoder = E.union Req.JsonSpec'
    static member JsonDecoder = D.union Req.JsonSpec'
    static member JsonSpec =
        FieldSpec.Create<Req> (Req.JsonEncoder, Req.JsonDecoder)
    interface IJson with
        member this.ToJson () = Req.JsonEncoder this
    interface IRequest with
        member this.Kind = Union.getKind<Req> this

type ClientRes =
    | OnJoin of JoinReq * StubResult<JoinRes, JoinErr>
    | OnAuth of AuthReq * StubResult<AuthRes, AuthErr>
    | OnLeave of LeaveReq * StubResult<LeaveRes, LeaveErr>
    | OnSetItem of SetItemReq * StubResult<SetItemRes, SetItemErr>
with
    static member StubSpec : Stub.ResponseSpec<ClientRes> list =
        [
            Stub.ResponseSpec<ClientRes>.Create ("DoJoin", [JoinReq.JsonSpec],
                "OnJoin", JoinRes.JsonDecoder, JoinErr.JsonDecoder)
            Stub.ResponseSpec<ClientRes>.Create ("DoAuth", [AuthReq.JsonSpec],
                "OnAuth", AuthRes.JsonDecoder, AuthErr.JsonDecoder)
            Stub.ResponseSpec<ClientRes>.Create ("DoLeave", [LeaveReq.JsonSpec],
                "OnLeave", LeaveRes.JsonDecoder, LeaveErr.JsonDecoder)
            Stub.ResponseSpec<ClientRes>.Create ("DoSetItem", [SetItemReq.JsonSpec],
                "OnSetItem", SetItemRes.JsonDecoder, SetItemErr.JsonDecoder)
        ]

type ServerReq =
    | DoJoin of JoinReq * Callback<Result<JoinRes, JoinErr>>
    | DoAuth of AuthReq * Callback<Result<AuthRes, AuthErr>>
    | DoLeave of LeaveReq * Callback<Result<LeaveRes, LeaveErr>>
    | DoSetItem of SetItemReq * Callback<Result<SetItemRes, SetItemErr>>
with
    static member HubSpec : Hub.RequestSpec<ServerReq> list =
        [
            Hub.RequestSpec<ServerReq>.Create ("DoJoin", [JoinReq.JsonSpec],
                Hub.getCallback<JoinRes, JoinErr>)
            Hub.RequestSpec<ServerReq>.Create ("DoAuth", [AuthReq.JsonSpec],
                Hub.getCallback<AuthRes, AuthErr>)
            Hub.RequestSpec<ServerReq>.Create ("DoLeave", [LeaveReq.JsonSpec],
                Hub.getCallback<LeaveRes, LeaveErr>)
            Hub.RequestSpec<ServerReq>.Create ("DoSetItem", [SetItemReq.JsonSpec],
                Hub.getCallback<SetItemRes, SetItemErr>)
        ]
    interface IReq

let StubSpec : StubSpec<Req, ClientRes, Evt> =
    {
        Response = ClientRes.StubSpec
        Event = Evt.JsonSpec'
    }