module SuperClip.Gui.Dsl.Prefabs

open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Context.Generator
open Dap.Platform
open Dap.Gui
open Dap.Gui.Builder
open Dap.Gui.Dsl.Prefab

let LinkStatus =
    h_stack {
        prefab "link_status"
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
        prefab "home_panel"
        child "link_status" LinkStatus
        child "history" (
            label {
                text "TODO"
            }
        )
    }

let AuthPanel =
    v_stack {
        prefab "auth_panel"
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

