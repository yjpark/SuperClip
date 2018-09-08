[<AutoOpen>]
module SuperClip.Core.Extensions

open System

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Content with
    member this.IsEmpty =
        match this with
        | Text text ->
            String.IsNullOrWhiteSpace text
    member this.Hash =
        if this.IsEmpty then
            ""
        else
            match this with
            | Text text -> calcSha256Sum text
    member this.Encrypt (cryptoKey : string) =
        match this with
        | Text text ->
            Des.encrypt cryptoKey text
            |> Text
    member this.Decrypt (runner : IRunner) (cryptoKey : string) =
        match this with
        | Text text ->
            Des.decrypt runner cryptoKey text
            |> Option.map (fun text ->
                Text text
            ) |> Option.defaultValue (Text "")

type Device with
    static member New name : Device =
        {
            Guid = (System.Guid.NewGuid()) .ToString ()
            Name = name
        }

type ChannelKey = string

type Channel with
    static member CalcGuid (name : string) =
        name
        |> fun n -> n.Trim ()
        |> calcSha256Sum2WithSalt "chiepuyiawaeR9aij6fiech7osh8kesh"
    static member CreateWithName name =
        {
            Guid = Channel.CalcGuid name
            Name = name
        }
    static member New name =
        {
            Guid = ""
            Name = name
        }
    member this.Key : ChannelKey =
        if this.Guid <> "" then
            this.Guid
        elif this.Name <> "" then
            Channel.CalcGuid this.Name
        else
            "_N/A_"

type Item with
    static member Empty =
        Item.Create Instant.MinValue Local (Text "")
    member this.IsEmpty = this.Content.IsEmpty
    member this.Hash = this.Content.Hash
    member this.ForCloud (peer : Peer) (cryptoKey : string) =
        {this with
            Source = Cloud peer
            Content = this.Content.Encrypt cryptoKey
        }
    member this.Decrypt (runner : IRunner) (cryptoKey : string) =
        let content = this.Content.Decrypt runner cryptoKey
        {this with Content = content}

