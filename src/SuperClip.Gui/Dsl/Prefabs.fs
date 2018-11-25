module SuperClip.Gui.Dsl.Prefabs

open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Context.Generator
open Dap.Platform
open Dap.Gui
open Dap.Gui.Builder
open Dap.Gui.Generator

let inputField labelText =
    input_field {
        update_label (fun l ->
            l.Text.SetValue labelText
        )
    }

let Clip =
    v_stack {
        child "content" (
            label {
                text "..."
            }
        )
        child "delete" (
            label {
                text "Delete"
            }
        )
        child "copy" (
            button {
                text "Copy"
            }
        )
    }

let Clips =
    f_table {
        item <@ Clip @>
    }

let LinkStatus =
    h_stack {
        child "link" (
            label {
                text "..."
            }
        )
        child "session" (
            label {
                text "..."
            }
        )
        child "action" (
            button {
                text "Action"
            }
        )
    }

let HomePanel =
    v_stack {
        child "link_status" <@ LinkStatus @>
        child "history" <@ Clips @>
    }

let AuthPanel =
    v_stack {
        styles ["style1" ; "style2"]
        child "title" (
            label {
                text "Please Provide Authentication Details"
            }
        )
        child "name" (inputField "User Name")
        child "device" (inputField "Device Name")
        child "password" (inputField "Password")
        child "cancel" (
            button {
                text "Cancel"
            }
        )
        child "auth" (
            button {
                text "Auth"
            }
        )
    }

let compile segments =
    [
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "Clip.fs"],
            "SuperClip.Gui.Prefab.Clip", <@ Clip @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "Clips.fs"],
            "SuperClip.Gui.Prefab.Clips", <@ Clips @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "LinkStatus.fs"],
            "SuperClip.Gui.Prefab.LinkStatus", <@ LinkStatus @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "AuthPanel.fs"],
            "SuperClip.Gui.Prefab.AuthPanel", <@ AuthPanel @>
        )
        G.PrefabFile (segments, ["_Gen" ; "Prefab" ; "HomePanel.fs"],
            "SuperClip.Gui.Prefab.HomePanel", <@ HomePanel @>
        )
    ]

