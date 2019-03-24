module SuperClip.Fabulous.Dsl.Text

open Dap.Context.Meta
open Dap.Context.Meta.Net
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Meta.Net
open Dap.Platform.Dsl.Packs
open Dap.Platform.Generator

let aboutContent = """
SuperClip is a clipboard manager

YJ Park
"""

let Common =
    combo {
        var (M.string ("ok", "Ok"))
        var (M.string ("this_device", "This Device"))
    }

let Home =
    combo {
        var (M.string ("title", "Super Clip"))
        var (M.string ("cloud_link", "Cloud Link"))
        var (M.string ("pinned_items", "Pinned Items"))
        var (M.string ("recent_items", "Recent Items"))
    }

let Link =
    combo {
        var (M.string ("login", "Login"))
        var (M.string ("logout", "Logout"))
        var (M.string ("connect", "Connect"))
        var (M.string ("logging_in", "Logging In ..."))
        var (M.string ("other_devices", "Other Devices: {0}"))
        var (M.string ("details", "Details"))
        var (M.string ("sync_with_others", "Sync with Others"))
    }

let Item =
    combo {
        var (M.string ("pin", "Pin"))
        var (M.string ("unpin", "Unpin"))
        var (M.string ("remove", "Remove"))
    }

let Settings =
    combo {
        var (M.string ("title", "Settings"))
        var (M.string ("sync_section", "Sync"))
        var (M.string ("display_section", "Display"))
        var (M.string ("cloud_mode", "Cloud Mode"))
        var (M.string ("reset_auth", "Reset"))
        var (M.string ("dark_theme", "Dark Theme"))
        var (M.string ("language_section", "Language"))
    }

let About =
    combo {
        var (M.string ("title", "About"))
        var (M.string ("content", aboutContent))
    }

let Auth =
    combo {
        var (M.string ("title", "Auth"))
        var (M.string ("channel", "Channel"))
        var (M.string ("device", "Device"))
        var (M.string ("password", "Password"))
        var (M.string ("login", "Login"))
    }

let Devices =
    combo {
        var (M.string ("title", "Devices"))
        var (M.string ("offline", "Offline"))
        var (M.string ("other_online_devices", "Other Online Devices"))
    }

let Resetting =
    combo {
        var (M.string ("title", "Resetting"))
        var (M.string ("theme_changed", "Theme has been changed"))
    }

let Help =
    combo {
        var (M.string ("title", "Help"))
        var (M.string ("home", "Home"))
        var (M.string ("auth", "Auth"))
        var (M.string ("settings", "Settings"))
        var (M.string ("devices", "Devices"))
    }

let Error =
    combo {
        var (M.string ("title", "Error"))
    }

let All =
    combo {
        var (M.custom (<@ Common @>, "common"))
        var (M.custom (<@ Home @>, "home"))
        var (M.custom (<@ Link @>, "link"))
        var (M.custom (<@ Item @>, "item"))
        var (M.custom (<@ Settings @>, "settings"))
        var (M.custom (<@ Auth @>, "auth"))
        var (M.custom (<@ About @>, "about"))
        var (M.custom (<@ Devices @>, "devices"))
        var (M.custom (<@ Resetting @>, "resetting"))
        var (M.custom (<@ Help @>, "help"))
        var (M.custom (<@ Error @>, "error"))
    }

let compile segments =
    [
        G.File (segments, ["_Gen" ; "Text.fs"],
            G.QualifiedModule ("SuperClip.Fabulous.Text",
                [
                    G.LooseJsonRecord <@ Common @>
                    G.LooseJsonRecord <@ Home @>
                    G.LooseJsonRecord <@ Link @>
                    G.LooseJsonRecord <@ Item @>
                    G.LooseJsonRecord <@ Settings @>
                    G.LooseJsonRecord <@ Auth @>
                    G.LooseJsonRecord <@ About @>
                    G.LooseJsonRecord <@ Devices @>
                    G.LooseJsonRecord <@ Resetting @>
                    G.LooseJsonRecord <@ Help @>
                    G.LooseJsonRecord <@ Error @>
                    G.LooseJsonRecord <@ All @>
                ]
            )
        )
    ]

