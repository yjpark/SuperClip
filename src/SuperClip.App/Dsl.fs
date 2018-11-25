module SuperClip.App.Dsl

open Dap.Context
open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.Platform.Dsl.Packs
open Dap.WebSocket.Meta
open Dap.Local.Dsl
open Dap.Remote.Meta

open SuperClip.Core.Dsl.Types
open SuperClip.Core.Dsl.Packs

let Credential =
    combo {
        var (M.custom (<@ Device @>, "device"))
        var (M.custom (<@ Channel @>, "channel"))
        var (M.string "pass_hash")
        var (M.string "crypto_key")
        var (M.string "token")
    }

let UserProps =
    combo {
        option (M.custom (<@ Credential @>, "credential"))
    }

let UserPref =
    context <@ UserProps @> {
        kind "UserPref"
    }

let ICloudStubPack =
    pack [ <@ ITickingPack @> ] {
        register_pack <@ ITickingPack @> (M.packetClient (logTraffic = true))
        add (M.proxy (
                aliases = [("Cloud", "SuperClip.Core.Cloud")],
                reqResEvt = "Cloud.Req, Cloud.ClientRes, Cloud.Evt",
                stubSpec = "Cloud.StubSpec",
                url = "(getCloudServerUri ())",
                retryDelay = 5.0<second>,
                logTraffic = true,
                kind = "CloudStub"
            ))
    }

let IClientPack =
    pack [ <@ ICorePack @> ; <@ ICloudStubPack @> ; <@ IAppPack @>] {
        add (M.context (<@ UserPref @>))
    }

let ISessionPack =
    pack [ <@ IClientPack @> ] {
        add_pack <@ IClientPack @> (
            M.agent (
                aliases = [("SessionTypes", "SuperClip.App.Session.Types")],
                args = M.noArgs,
                type' = "SessionTypes.Agent",
                spec = "SuperClip.App.Session.Logic.spec",
                kind = "Session"
            )
        )
    }

let AppGui =
    emptyContext {
        kind "AppGui"
        handler (M.unit "do_login") (M.unit response)
    }

let IGuiPack =
    pack [] {
        add (M.feature ("IAppGui"))
    }

let App =
    live {
        has <@ ISessionPack @>
        has <@ IGuiPack @>
    }

type G with
    static member GuiPack (feature : string option) =
        let feature = defaultArg feature "SuperClip.App.Feature"
        [
            sprintf "type AppGui = %s.AppGui.Context" feature
        ]

let commonLines =
    [
        G.PackOpens
        [
            "open Dap.Local"
            "open SuperClip.Core"
        ]
    ]|> concatSections

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.App.Types",
                [
                    commonLines
                    G.JsonRecord <@ Credential @>
                    G.Combo (<@ UserProps @>)
                    G.Context <@ UserPref @>
                    G.PackInterface <@ ICloudStubPack @>
                    G.PackInterface <@ IClientPack @>
                    G.Feature <@ AppGui @>
                    G.PackInterface <@ IGuiPack @>
                ]
            )
        )
        G.File (segments, ["_Gen1" ; "Packs.fs"],
            G.AutoOpenModule ("SuperClip.App.Packs",
                [
                    commonLines
                    G.PackInterface <@ ISessionPack @>
                ]
            )
        )
        G.File (segments, ["_Gen1"; "IApp.fs"],
            G.AutoOpenModule ("SuperClip.App.IApp",
                [
                    commonLines
                    G.AppInterface <@ App @>
                ]
            )
        )
        G.File (segments, ["_Gen1"; "App.fs"],
            G.AutoOpenModule ("SuperClip.App.App",
                [
                    commonLines
                    G.AppClass <@ App @>
                ]
            )
        )
    ]
