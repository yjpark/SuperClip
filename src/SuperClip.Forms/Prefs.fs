[<RequireQualifiedAccess>]
module SuperClip.Forms.Prefs

open Dap.Prelude
open Dap.Platform
open Dap.Remote

open SuperClip.Core

type Record = {
    Device : Device
    Channels : Channel list
    Tokens : Map<string, string>
} with
    static member Create device channels tokens = {
        Device = device
        Channels = channels
        Tokens = tokens
    }
    static member New name =
        let guid = (System.Guid.NewGuid ()) .ToString ()
        {
            Device = Device.Create guid name
            Channels = []
            Tokens = Map.empty
        }
    static member JsonDecoder =
        D.decode Record.Create
        |> D.required "device" Device.JsonDecoder
        |> D.optional "channels" (D.list Channel.JsonDecoder) []
        |> D.optional "tokens" (D.dict D.string) Map.empty
    static member JsonEncoder (this : Record) =
        E.object [
            "device", Device.JsonEncoder this.Device
            "channels", E.list <| List.map Channel.JsonEncoder this.Channels
            "tokens", E.dict <| Map.map (fun k v -> E.string v) this.Tokens
        ]
    interface IJson with
        member this.ToJson () =
            Record.JsonEncoder this
