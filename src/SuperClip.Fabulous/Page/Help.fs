[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Help

open Xamarin.Forms

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Remote
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let render (runner : View) (topic : HelpTopic) =
    let pkt = """{"t":"2019-03-21T03:33:38.3787087Z","i":"323a2265-21de-4d60-b9d4-b0d5afe274c1","k":"Res","p":"eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJqdGkiOiIyODdmOTkxYS1hN2JjLTQ2M2EtOTNlZi02NzQwZGJiMjJiODkiLCJzdWIiOiI2YjhjM2VmYzkzMTQxODZkNTEzNDY5OTYyNWYxM2Q4Yzg1YTFiMjcwNmM1M2FmODJjMzkwZDAyNmU2NjgxNDQzIiwia2V5IjoiIiwiZGF0Ijp7Imd1aWQiOiIwZTVlZTQwMS1kNTJiLTRjOTEtOTM5Yy01NTVmMzc5Yjg5ZDQiLCJuYW1lIjoiU00tVDgzNUMifSwiaWF0IjoiMTU1MzEzOTIxOCIsImV4cCI6IjE1NTMyMjU2MTgifQ."}"""
    let json = tryDecodeJson Dap.Remote.Internal.Types.Packet.JsonDecoder pkt
    let text1 =
        try
            encodeJson 4 json.Value
        with e ->
            logException runner "AAAAAAAAAAAAAAAAAAAAAAAAAA" pkt (json) e
            e.ToString ()
    let bytes = System.Text.Encoding.UTF8.GetBytes pkt
    let text2 =
        try
            let json = Dap.Remote.WebSocketGateway.PacketConn.decode (bytes, 0, bytes.Length)
            encodeJson 4 json
            //(parseJson pkt) .ToString ()
        with e ->
            logException runner "AAAAAAAAAAAAAAAAAAAAAAAAAA 2" pkt () e
            e.ToString ()
    v_box {
        children [
            label {
                text text1
            }
            label {
                text text2
            }
            button {
                classId Theme.Button_Big
                text "Ok"
                command (fun _ ->
                    runner.React <| DoSetHelp None
                )
            }
        ]
    }|> scrollPage "Help"