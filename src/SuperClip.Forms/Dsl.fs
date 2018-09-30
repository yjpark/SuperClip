module SuperClip.Forms.Dsl

open Dap.Context
open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator
open Dap.WebSocket.Meta
open Dap.Remote.Meta
open Dap.Forms.Meta

open SuperClip.Core.Dsl

let Credential =
    combo {
        var (M.custom (<@ Device @>, "device"))
        var (M.custom (<@ Channel @>, "channel"))
        var (M.string "pass_hash")
        var (M.string "crypto_key")
        var (M.string "token")
    }

let PrefProperties =
    combo {
        option (M.custom (<@ Credential @>, "credential"))
    }

let PrefContext =
    [
        "type PrefContext = CustomContext<PrefProperties>"
        ""
        "let spawnPrefContext (runner : IAgent) ="
        "    CustomContext<PrefProperties> (runner.Env.Logging, runner.Ident.Kind, PrefProperties.Create)"
    ]

let ICloudStubPack =
    pack [] {
        register (M.packetClientSpawner (true))
        add (M.proxyService (
                [("CloudTypes", "SuperClip.Core.Cloud.Types")],
                "CloudTypes.Req, CloudTypes.ClientRes, CloudTypes.Evt", "CloudTypes.StubSpec",
                "(getCloudServerUri ())", Some 5.0<second>, true,
                "CloudStub", NoKey
            ))
    }

let IClientPack =
    pack [ <@ ICorePack @> ; <@ ICloudStubPack @> ] {
        add (M.secureStorageService ([], "Credential", "credential"))
        add (M.contextService ([], "PrefContext", "spawnPrefContext", "preferences"))
    }

let ISessionPack =
    pack [ <@ IClientPack @> ] {
        add_pack <@ IClientPack @> (
            M.service (
                [("SessionTypes", "SuperClip.Forms.Session.Types")],
                M.noArgs, "SessionTypes.Agent", "SuperClip.Forms.Session.Logic.spec", "Session"
            ))
    }

let IAppPack =
    pack [ <@ ISessionPack @> ] {
        add_pack <@ ISessionPack @> (
            M.formsViewService (
                [("ViewTypes", "SuperClip.Forms.View.Types")],
                "ISessionPack, ViewTypes.Model, ViewTypes.Msg", "(SuperClip.Forms.View.Logic.newArgs ())"
            ))
    }

let App =
    live {
        has <@ IAppPack @>
    }

let coreOpens =
    [
        "open SuperClip.Core"
    ]

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Types.fs"],
            G.AutoOpenModule ("SuperClip.Forms.Types",
                [
                    G.PackOpens
                    coreOpens
                    G.JsonRecord <@ Credential @>
                    G.FinalClass (<@ PrefProperties @>)
                    PrefContext
                    G.PackInterface <@ ICloudStubPack @>
                    G.PackInterface <@ IClientPack @>
                ]
            )
        )
        G.File (segments, ["_Gen" ; "Packs.fs"],
            G.AutoOpenModule ("SuperClip.Forms.Packs",
                [
                    G.PackOpens
                    coreOpens
                    G.PackInterface <@ ISessionPack @>
                ]
            )
        )
        G.File (segments, ["_Gen"; "App.fs"],
            G.AutoOpenModule ("SuperClip.Forms.App",
                [
                    G.PackOpens
                    coreOpens
                    G.PackInterface <@ IAppPack @>
                    G.App <@ App @>
                ]
            )
        )
    ]
