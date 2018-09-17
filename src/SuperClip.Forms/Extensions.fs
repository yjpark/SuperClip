[<AutoOpen>]
module SuperClip.Forms.Extensions

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Credential with
    member this.Self =
        Peer.Create this.Channel this.Device

type ICorePack with
    member this.Primary = this.PrimaryClipboard
    member this.History = this.LocalHistory

type ICloudStubPack with
    member this.Stub = this.CloudStub

type IClientPack with
    member this.Pref = this.Preferences.Context
