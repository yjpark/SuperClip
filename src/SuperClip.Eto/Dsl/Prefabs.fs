module SuperClip.Eto.Dsl.Prefabs

open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Context.Generator
open Dap.Platform
open Dap.Gui
open Dap.Gui.Builder

open Dap.Eto.Builder
open Dap.Eto.Generator
open SuperClip.Gui.Dsl.Prefabs

let compile segments =
    [
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "LinkStatus.fs"],
            "SuperClip.Prefab.LinkStatus", <@ LinkStatus @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "AuthPanel.fs"],
            "SuperClip.Prefab.AuthPanel", <@ AuthPanel @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "HomePanel.fs"],
            "SuperClip.Prefab.HomePanel", <@ HomePanel @>
        )
    ]

