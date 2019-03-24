[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Text

open Dap.Prelude
open Dap.Context

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Common = {
    Ok : (* Common *) string
    ThisDevice : (* Common *) string
} with
    static member Create
        (
            ?ok : (* Common *) string,
            ?thisDevice : (* Common *) string
        ) : Common =
        {
            Ok = (* Common *) ok
                |> Option.defaultWith (fun () -> "Ok")
            ThisDevice = (* Common *) thisDevice
                |> Option.defaultWith (fun () -> "This Device")
        }
    static member SetOk ((* Common *) ok : string) (this : Common) =
        {this with Ok = ok}
    static member SetThisDevice ((* Common *) thisDevice : string) (this : Common) =
        {this with ThisDevice = thisDevice}
    static member JsonEncoder : JsonEncoder<Common> =
        fun (this : Common) ->
            E.object [
                "ok", E.string (* Common *) this.Ok
                "this_device", E.string (* Common *) this.ThisDevice
            ]
    static member JsonDecoder : JsonDecoder<Common> =
        D.object (fun get ->
            {
                Ok = get.Optional.Field (* Common *) "ok" D.string
                    |> Option.defaultValue "Ok"
                ThisDevice = get.Optional.Field (* Common *) "this_device" D.string
                    |> Option.defaultValue "This Device"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Common> (Common.JsonEncoder, Common.JsonDecoder)
    interface IJson with
        member this.ToJson () = Common.JsonEncoder this
    interface IObj
    member this.WithOk ((* Common *) ok : string) =
        this |> Common.SetOk ok
    member this.WithThisDevice ((* Common *) thisDevice : string) =
        this |> Common.SetThisDevice thisDevice

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Home = {
    Title : (* Home *) string
    CloudLink : (* Home *) string
    PinnedItems : (* Home *) string
    RecentItems : (* Home *) string
} with
    static member Create
        (
            ?title : (* Home *) string,
            ?cloudLink : (* Home *) string,
            ?pinnedItems : (* Home *) string,
            ?recentItems : (* Home *) string
        ) : Home =
        {
            Title = (* Home *) title
                |> Option.defaultWith (fun () -> "Super Clip")
            CloudLink = (* Home *) cloudLink
                |> Option.defaultWith (fun () -> "Cloud Link")
            PinnedItems = (* Home *) pinnedItems
                |> Option.defaultWith (fun () -> "Pinned Items")
            RecentItems = (* Home *) recentItems
                |> Option.defaultWith (fun () -> "Recent Items")
        }
    static member SetTitle ((* Home *) title : string) (this : Home) =
        {this with Title = title}
    static member SetCloudLink ((* Home *) cloudLink : string) (this : Home) =
        {this with CloudLink = cloudLink}
    static member SetPinnedItems ((* Home *) pinnedItems : string) (this : Home) =
        {this with PinnedItems = pinnedItems}
    static member SetRecentItems ((* Home *) recentItems : string) (this : Home) =
        {this with RecentItems = recentItems}
    static member JsonEncoder : JsonEncoder<Home> =
        fun (this : Home) ->
            E.object [
                "title", E.string (* Home *) this.Title
                "cloud_link", E.string (* Home *) this.CloudLink
                "pinned_items", E.string (* Home *) this.PinnedItems
                "recent_items", E.string (* Home *) this.RecentItems
            ]
    static member JsonDecoder : JsonDecoder<Home> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Home *) "title" D.string
                    |> Option.defaultValue "Super Clip"
                CloudLink = get.Optional.Field (* Home *) "cloud_link" D.string
                    |> Option.defaultValue "Cloud Link"
                PinnedItems = get.Optional.Field (* Home *) "pinned_items" D.string
                    |> Option.defaultValue "Pinned Items"
                RecentItems = get.Optional.Field (* Home *) "recent_items" D.string
                    |> Option.defaultValue "Recent Items"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Home> (Home.JsonEncoder, Home.JsonDecoder)
    interface IJson with
        member this.ToJson () = Home.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Home *) title : string) =
        this |> Home.SetTitle title
    member this.WithCloudLink ((* Home *) cloudLink : string) =
        this |> Home.SetCloudLink cloudLink
    member this.WithPinnedItems ((* Home *) pinnedItems : string) =
        this |> Home.SetPinnedItems pinnedItems
    member this.WithRecentItems ((* Home *) recentItems : string) =
        this |> Home.SetRecentItems recentItems

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Link = {
    Login : (* Link *) string
    Logout : (* Link *) string
    Connect : (* Link *) string
    LoggingIn : (* Link *) string
    OtherDevices : (* Link *) string
    Details : (* Link *) string
    SyncWithOthers : (* Link *) string
} with
    static member Create
        (
            ?login : (* Link *) string,
            ?logout : (* Link *) string,
            ?connect : (* Link *) string,
            ?loggingIn : (* Link *) string,
            ?otherDevices : (* Link *) string,
            ?details : (* Link *) string,
            ?syncWithOthers : (* Link *) string
        ) : Link =
        {
            Login = (* Link *) login
                |> Option.defaultWith (fun () -> "Login")
            Logout = (* Link *) logout
                |> Option.defaultWith (fun () -> "Logout")
            Connect = (* Link *) connect
                |> Option.defaultWith (fun () -> "Connect")
            LoggingIn = (* Link *) loggingIn
                |> Option.defaultWith (fun () -> "Logging In ...")
            OtherDevices = (* Link *) otherDevices
                |> Option.defaultWith (fun () -> "Other Devices: {0}")
            Details = (* Link *) details
                |> Option.defaultWith (fun () -> "Details")
            SyncWithOthers = (* Link *) syncWithOthers
                |> Option.defaultWith (fun () -> "Sync with Others")
        }
    static member SetLogin ((* Link *) login : string) (this : Link) =
        {this with Login = login}
    static member SetLogout ((* Link *) logout : string) (this : Link) =
        {this with Logout = logout}
    static member SetConnect ((* Link *) connect : string) (this : Link) =
        {this with Connect = connect}
    static member SetLoggingIn ((* Link *) loggingIn : string) (this : Link) =
        {this with LoggingIn = loggingIn}
    static member SetOtherDevices ((* Link *) otherDevices : string) (this : Link) =
        {this with OtherDevices = otherDevices}
    static member SetDetails ((* Link *) details : string) (this : Link) =
        {this with Details = details}
    static member SetSyncWithOthers ((* Link *) syncWithOthers : string) (this : Link) =
        {this with SyncWithOthers = syncWithOthers}
    static member JsonEncoder : JsonEncoder<Link> =
        fun (this : Link) ->
            E.object [
                "login", E.string (* Link *) this.Login
                "logout", E.string (* Link *) this.Logout
                "connect", E.string (* Link *) this.Connect
                "logging_in", E.string (* Link *) this.LoggingIn
                "other_devices", E.string (* Link *) this.OtherDevices
                "details", E.string (* Link *) this.Details
                "sync_with_others", E.string (* Link *) this.SyncWithOthers
            ]
    static member JsonDecoder : JsonDecoder<Link> =
        D.object (fun get ->
            {
                Login = get.Optional.Field (* Link *) "login" D.string
                    |> Option.defaultValue "Login"
                Logout = get.Optional.Field (* Link *) "logout" D.string
                    |> Option.defaultValue "Logout"
                Connect = get.Optional.Field (* Link *) "connect" D.string
                    |> Option.defaultValue "Connect"
                LoggingIn = get.Optional.Field (* Link *) "logging_in" D.string
                    |> Option.defaultValue "Logging In ..."
                OtherDevices = get.Optional.Field (* Link *) "other_devices" D.string
                    |> Option.defaultValue "Other Devices: {0}"
                Details = get.Optional.Field (* Link *) "details" D.string
                    |> Option.defaultValue "Details"
                SyncWithOthers = get.Optional.Field (* Link *) "sync_with_others" D.string
                    |> Option.defaultValue "Sync with Others"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Link> (Link.JsonEncoder, Link.JsonDecoder)
    interface IJson with
        member this.ToJson () = Link.JsonEncoder this
    interface IObj
    member this.WithLogin ((* Link *) login : string) =
        this |> Link.SetLogin login
    member this.WithLogout ((* Link *) logout : string) =
        this |> Link.SetLogout logout
    member this.WithConnect ((* Link *) connect : string) =
        this |> Link.SetConnect connect
    member this.WithLoggingIn ((* Link *) loggingIn : string) =
        this |> Link.SetLoggingIn loggingIn
    member this.WithOtherDevices ((* Link *) otherDevices : string) =
        this |> Link.SetOtherDevices otherDevices
    member this.WithDetails ((* Link *) details : string) =
        this |> Link.SetDetails details
    member this.WithSyncWithOthers ((* Link *) syncWithOthers : string) =
        this |> Link.SetSyncWithOthers syncWithOthers

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Item = {
    Pin : (* Item *) string
    Unpin : (* Item *) string
    Remove : (* Item *) string
} with
    static member Create
        (
            ?pin : (* Item *) string,
            ?unpin : (* Item *) string,
            ?remove : (* Item *) string
        ) : Item =
        {
            Pin = (* Item *) pin
                |> Option.defaultWith (fun () -> "Pin")
            Unpin = (* Item *) unpin
                |> Option.defaultWith (fun () -> "Unpin")
            Remove = (* Item *) remove
                |> Option.defaultWith (fun () -> "Remove")
        }
    static member SetPin ((* Item *) pin : string) (this : Item) =
        {this with Pin = pin}
    static member SetUnpin ((* Item *) unpin : string) (this : Item) =
        {this with Unpin = unpin}
    static member SetRemove ((* Item *) remove : string) (this : Item) =
        {this with Remove = remove}
    static member JsonEncoder : JsonEncoder<Item> =
        fun (this : Item) ->
            E.object [
                "pin", E.string (* Item *) this.Pin
                "unpin", E.string (* Item *) this.Unpin
                "remove", E.string (* Item *) this.Remove
            ]
    static member JsonDecoder : JsonDecoder<Item> =
        D.object (fun get ->
            {
                Pin = get.Optional.Field (* Item *) "pin" D.string
                    |> Option.defaultValue "Pin"
                Unpin = get.Optional.Field (* Item *) "unpin" D.string
                    |> Option.defaultValue "Unpin"
                Remove = get.Optional.Field (* Item *) "remove" D.string
                    |> Option.defaultValue "Remove"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Item> (Item.JsonEncoder, Item.JsonDecoder)
    interface IJson with
        member this.ToJson () = Item.JsonEncoder this
    interface IObj
    member this.WithPin ((* Item *) pin : string) =
        this |> Item.SetPin pin
    member this.WithUnpin ((* Item *) unpin : string) =
        this |> Item.SetUnpin unpin
    member this.WithRemove ((* Item *) remove : string) =
        this |> Item.SetRemove remove

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Settings = {
    Title : (* Settings *) string
    SyncSection : (* Settings *) string
    DisplaySection : (* Settings *) string
    CloudMode : (* Settings *) string
    ResetAuth : (* Settings *) string
    DarkTheme : (* Settings *) string
    LanguageSection : (* Settings *) string
} with
    static member Create
        (
            ?title : (* Settings *) string,
            ?syncSection : (* Settings *) string,
            ?displaySection : (* Settings *) string,
            ?cloudMode : (* Settings *) string,
            ?resetAuth : (* Settings *) string,
            ?darkTheme : (* Settings *) string,
            ?languageSection : (* Settings *) string
        ) : Settings =
        {
            Title = (* Settings *) title
                |> Option.defaultWith (fun () -> "Settings")
            SyncSection = (* Settings *) syncSection
                |> Option.defaultWith (fun () -> "Sync")
            DisplaySection = (* Settings *) displaySection
                |> Option.defaultWith (fun () -> "Display")
            CloudMode = (* Settings *) cloudMode
                |> Option.defaultWith (fun () -> "Cloud Mode")
            ResetAuth = (* Settings *) resetAuth
                |> Option.defaultWith (fun () -> "Reset")
            DarkTheme = (* Settings *) darkTheme
                |> Option.defaultWith (fun () -> "Dark Theme")
            LanguageSection = (* Settings *) languageSection
                |> Option.defaultWith (fun () -> "Language")
        }
    static member SetTitle ((* Settings *) title : string) (this : Settings) =
        {this with Title = title}
    static member SetSyncSection ((* Settings *) syncSection : string) (this : Settings) =
        {this with SyncSection = syncSection}
    static member SetDisplaySection ((* Settings *) displaySection : string) (this : Settings) =
        {this with DisplaySection = displaySection}
    static member SetCloudMode ((* Settings *) cloudMode : string) (this : Settings) =
        {this with CloudMode = cloudMode}
    static member SetResetAuth ((* Settings *) resetAuth : string) (this : Settings) =
        {this with ResetAuth = resetAuth}
    static member SetDarkTheme ((* Settings *) darkTheme : string) (this : Settings) =
        {this with DarkTheme = darkTheme}
    static member SetLanguageSection ((* Settings *) languageSection : string) (this : Settings) =
        {this with LanguageSection = languageSection}
    static member JsonEncoder : JsonEncoder<Settings> =
        fun (this : Settings) ->
            E.object [
                "title", E.string (* Settings *) this.Title
                "sync_section", E.string (* Settings *) this.SyncSection
                "display_section", E.string (* Settings *) this.DisplaySection
                "cloud_mode", E.string (* Settings *) this.CloudMode
                "reset_auth", E.string (* Settings *) this.ResetAuth
                "dark_theme", E.string (* Settings *) this.DarkTheme
                "language_section", E.string (* Settings *) this.LanguageSection
            ]
    static member JsonDecoder : JsonDecoder<Settings> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Settings *) "title" D.string
                    |> Option.defaultValue "Settings"
                SyncSection = get.Optional.Field (* Settings *) "sync_section" D.string
                    |> Option.defaultValue "Sync"
                DisplaySection = get.Optional.Field (* Settings *) "display_section" D.string
                    |> Option.defaultValue "Display"
                CloudMode = get.Optional.Field (* Settings *) "cloud_mode" D.string
                    |> Option.defaultValue "Cloud Mode"
                ResetAuth = get.Optional.Field (* Settings *) "reset_auth" D.string
                    |> Option.defaultValue "Reset"
                DarkTheme = get.Optional.Field (* Settings *) "dark_theme" D.string
                    |> Option.defaultValue "Dark Theme"
                LanguageSection = get.Optional.Field (* Settings *) "language_section" D.string
                    |> Option.defaultValue "Language"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Settings> (Settings.JsonEncoder, Settings.JsonDecoder)
    interface IJson with
        member this.ToJson () = Settings.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Settings *) title : string) =
        this |> Settings.SetTitle title
    member this.WithSyncSection ((* Settings *) syncSection : string) =
        this |> Settings.SetSyncSection syncSection
    member this.WithDisplaySection ((* Settings *) displaySection : string) =
        this |> Settings.SetDisplaySection displaySection
    member this.WithCloudMode ((* Settings *) cloudMode : string) =
        this |> Settings.SetCloudMode cloudMode
    member this.WithResetAuth ((* Settings *) resetAuth : string) =
        this |> Settings.SetResetAuth resetAuth
    member this.WithDarkTheme ((* Settings *) darkTheme : string) =
        this |> Settings.SetDarkTheme darkTheme
    member this.WithLanguageSection ((* Settings *) languageSection : string) =
        this |> Settings.SetLanguageSection languageSection

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Auth = {
    Title : (* Auth *) string
    Channel : (* Auth *) string
    Device : (* Auth *) string
    Password : (* Auth *) string
    Login : (* Auth *) string
} with
    static member Create
        (
            ?title : (* Auth *) string,
            ?channel : (* Auth *) string,
            ?device : (* Auth *) string,
            ?password : (* Auth *) string,
            ?login : (* Auth *) string
        ) : Auth =
        {
            Title = (* Auth *) title
                |> Option.defaultWith (fun () -> "Auth")
            Channel = (* Auth *) channel
                |> Option.defaultWith (fun () -> "Channel")
            Device = (* Auth *) device
                |> Option.defaultWith (fun () -> "Device")
            Password = (* Auth *) password
                |> Option.defaultWith (fun () -> "Password")
            Login = (* Auth *) login
                |> Option.defaultWith (fun () -> "Login")
        }
    static member SetTitle ((* Auth *) title : string) (this : Auth) =
        {this with Title = title}
    static member SetChannel ((* Auth *) channel : string) (this : Auth) =
        {this with Channel = channel}
    static member SetDevice ((* Auth *) device : string) (this : Auth) =
        {this with Device = device}
    static member SetPassword ((* Auth *) password : string) (this : Auth) =
        {this with Password = password}
    static member SetLogin ((* Auth *) login : string) (this : Auth) =
        {this with Login = login}
    static member JsonEncoder : JsonEncoder<Auth> =
        fun (this : Auth) ->
            E.object [
                "title", E.string (* Auth *) this.Title
                "channel", E.string (* Auth *) this.Channel
                "device", E.string (* Auth *) this.Device
                "password", E.string (* Auth *) this.Password
                "login", E.string (* Auth *) this.Login
            ]
    static member JsonDecoder : JsonDecoder<Auth> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Auth *) "title" D.string
                    |> Option.defaultValue "Auth"
                Channel = get.Optional.Field (* Auth *) "channel" D.string
                    |> Option.defaultValue "Channel"
                Device = get.Optional.Field (* Auth *) "device" D.string
                    |> Option.defaultValue "Device"
                Password = get.Optional.Field (* Auth *) "password" D.string
                    |> Option.defaultValue "Password"
                Login = get.Optional.Field (* Auth *) "login" D.string
                    |> Option.defaultValue "Login"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Auth> (Auth.JsonEncoder, Auth.JsonDecoder)
    interface IJson with
        member this.ToJson () = Auth.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Auth *) title : string) =
        this |> Auth.SetTitle title
    member this.WithChannel ((* Auth *) channel : string) =
        this |> Auth.SetChannel channel
    member this.WithDevice ((* Auth *) device : string) =
        this |> Auth.SetDevice device
    member this.WithPassword ((* Auth *) password : string) =
        this |> Auth.SetPassword password
    member this.WithLogin ((* Auth *) login : string) =
        this |> Auth.SetLogin login

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type About = {
    Title : (* About *) string
    Content : (* About *) string
} with
    static member Create
        (
            ?title : (* About *) string,
            ?content : (* About *) string
        ) : About =
        {
            Title = (* About *) title
                |> Option.defaultWith (fun () -> "About")
            Content = (* About *) content
                |> Option.defaultWith (fun () -> "
SuperClip is a clipboard manager

YJ Park
")
        }
    static member SetTitle ((* About *) title : string) (this : About) =
        {this with Title = title}
    static member SetContent ((* About *) content : string) (this : About) =
        {this with Content = content}
    static member JsonEncoder : JsonEncoder<About> =
        fun (this : About) ->
            E.object [
                "title", E.string (* About *) this.Title
                "content", E.string (* About *) this.Content
            ]
    static member JsonDecoder : JsonDecoder<About> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* About *) "title" D.string
                    |> Option.defaultValue "About"
                Content = get.Optional.Field (* About *) "content" D.string
                    |> Option.defaultValue "
SuperClip is a clipboard manager

YJ Park
"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<About> (About.JsonEncoder, About.JsonDecoder)
    interface IJson with
        member this.ToJson () = About.JsonEncoder this
    interface IObj
    member this.WithTitle ((* About *) title : string) =
        this |> About.SetTitle title
    member this.WithContent ((* About *) content : string) =
        this |> About.SetContent content

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Devices = {
    Title : (* Devices *) string
    Offline : (* Devices *) string
    OtherOnlineDevices : (* Devices *) string
} with
    static member Create
        (
            ?title : (* Devices *) string,
            ?offline : (* Devices *) string,
            ?otherOnlineDevices : (* Devices *) string
        ) : Devices =
        {
            Title = (* Devices *) title
                |> Option.defaultWith (fun () -> "Devices")
            Offline = (* Devices *) offline
                |> Option.defaultWith (fun () -> "Offline")
            OtherOnlineDevices = (* Devices *) otherOnlineDevices
                |> Option.defaultWith (fun () -> "Other Online Devices")
        }
    static member SetTitle ((* Devices *) title : string) (this : Devices) =
        {this with Title = title}
    static member SetOffline ((* Devices *) offline : string) (this : Devices) =
        {this with Offline = offline}
    static member SetOtherOnlineDevices ((* Devices *) otherOnlineDevices : string) (this : Devices) =
        {this with OtherOnlineDevices = otherOnlineDevices}
    static member JsonEncoder : JsonEncoder<Devices> =
        fun (this : Devices) ->
            E.object [
                "title", E.string (* Devices *) this.Title
                "offline", E.string (* Devices *) this.Offline
                "other_online_devices", E.string (* Devices *) this.OtherOnlineDevices
            ]
    static member JsonDecoder : JsonDecoder<Devices> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Devices *) "title" D.string
                    |> Option.defaultValue "Devices"
                Offline = get.Optional.Field (* Devices *) "offline" D.string
                    |> Option.defaultValue "Offline"
                OtherOnlineDevices = get.Optional.Field (* Devices *) "other_online_devices" D.string
                    |> Option.defaultValue "Other Online Devices"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Devices> (Devices.JsonEncoder, Devices.JsonDecoder)
    interface IJson with
        member this.ToJson () = Devices.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Devices *) title : string) =
        this |> Devices.SetTitle title
    member this.WithOffline ((* Devices *) offline : string) =
        this |> Devices.SetOffline offline
    member this.WithOtherOnlineDevices ((* Devices *) otherOnlineDevices : string) =
        this |> Devices.SetOtherOnlineDevices otherOnlineDevices

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Resetting = {
    Title : (* Resetting *) string
    ThemeChanged : (* Resetting *) string
} with
    static member Create
        (
            ?title : (* Resetting *) string,
            ?themeChanged : (* Resetting *) string
        ) : Resetting =
        {
            Title = (* Resetting *) title
                |> Option.defaultWith (fun () -> "Resetting")
            ThemeChanged = (* Resetting *) themeChanged
                |> Option.defaultWith (fun () -> "Theme has been changed")
        }
    static member SetTitle ((* Resetting *) title : string) (this : Resetting) =
        {this with Title = title}
    static member SetThemeChanged ((* Resetting *) themeChanged : string) (this : Resetting) =
        {this with ThemeChanged = themeChanged}
    static member JsonEncoder : JsonEncoder<Resetting> =
        fun (this : Resetting) ->
            E.object [
                "title", E.string (* Resetting *) this.Title
                "theme_changed", E.string (* Resetting *) this.ThemeChanged
            ]
    static member JsonDecoder : JsonDecoder<Resetting> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Resetting *) "title" D.string
                    |> Option.defaultValue "Resetting"
                ThemeChanged = get.Optional.Field (* Resetting *) "theme_changed" D.string
                    |> Option.defaultValue "Theme has been changed"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Resetting> (Resetting.JsonEncoder, Resetting.JsonDecoder)
    interface IJson with
        member this.ToJson () = Resetting.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Resetting *) title : string) =
        this |> Resetting.SetTitle title
    member this.WithThemeChanged ((* Resetting *) themeChanged : string) =
        this |> Resetting.SetThemeChanged themeChanged

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Help = {
    Title : (* Help *) string
    Home : (* Help *) string
    Auth : (* Help *) string
    Settings : (* Help *) string
    Devices : (* Help *) string
} with
    static member Create
        (
            ?title : (* Help *) string,
            ?home : (* Help *) string,
            ?auth : (* Help *) string,
            ?settings : (* Help *) string,
            ?devices : (* Help *) string
        ) : Help =
        {
            Title = (* Help *) title
                |> Option.defaultWith (fun () -> "Help")
            Home = (* Help *) home
                |> Option.defaultWith (fun () -> "Home")
            Auth = (* Help *) auth
                |> Option.defaultWith (fun () -> "Auth")
            Settings = (* Help *) settings
                |> Option.defaultWith (fun () -> "Settings")
            Devices = (* Help *) devices
                |> Option.defaultWith (fun () -> "Devices")
        }
    static member SetTitle ((* Help *) title : string) (this : Help) =
        {this with Title = title}
    static member SetHome ((* Help *) home : string) (this : Help) =
        {this with Home = home}
    static member SetAuth ((* Help *) auth : string) (this : Help) =
        {this with Auth = auth}
    static member SetSettings ((* Help *) settings : string) (this : Help) =
        {this with Settings = settings}
    static member SetDevices ((* Help *) devices : string) (this : Help) =
        {this with Devices = devices}
    static member JsonEncoder : JsonEncoder<Help> =
        fun (this : Help) ->
            E.object [
                "title", E.string (* Help *) this.Title
                "home", E.string (* Help *) this.Home
                "auth", E.string (* Help *) this.Auth
                "settings", E.string (* Help *) this.Settings
                "devices", E.string (* Help *) this.Devices
            ]
    static member JsonDecoder : JsonDecoder<Help> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Help *) "title" D.string
                    |> Option.defaultValue "Help"
                Home = get.Optional.Field (* Help *) "home" D.string
                    |> Option.defaultValue "Home"
                Auth = get.Optional.Field (* Help *) "auth" D.string
                    |> Option.defaultValue "Auth"
                Settings = get.Optional.Field (* Help *) "settings" D.string
                    |> Option.defaultValue "Settings"
                Devices = get.Optional.Field (* Help *) "devices" D.string
                    |> Option.defaultValue "Devices"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Help> (Help.JsonEncoder, Help.JsonDecoder)
    interface IJson with
        member this.ToJson () = Help.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Help *) title : string) =
        this |> Help.SetTitle title
    member this.WithHome ((* Help *) home : string) =
        this |> Help.SetHome home
    member this.WithAuth ((* Help *) auth : string) =
        this |> Help.SetAuth auth
    member this.WithSettings ((* Help *) settings : string) =
        this |> Help.SetSettings settings
    member this.WithDevices ((* Help *) devices : string) =
        this |> Help.SetDevices devices

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type Error = {
    Title : (* Error *) string
} with
    static member Create
        (
            ?title : (* Error *) string
        ) : Error =
        {
            Title = (* Error *) title
                |> Option.defaultWith (fun () -> "Error")
        }
    static member SetTitle ((* Error *) title : string) (this : Error) =
        {this with Title = title}
    static member JsonEncoder : JsonEncoder<Error> =
        fun (this : Error) ->
            E.object [
                "title", E.string (* Error *) this.Title
            ]
    static member JsonDecoder : JsonDecoder<Error> =
        D.object (fun get ->
            {
                Title = get.Optional.Field (* Error *) "title" D.string
                    |> Option.defaultValue "Error"
            }
        )
    static member JsonSpec =
        FieldSpec.Create<Error> (Error.JsonEncoder, Error.JsonDecoder)
    interface IJson with
        member this.ToJson () = Error.JsonEncoder this
    interface IObj
    member this.WithTitle ((* Error *) title : string) =
        this |> Error.SetTitle title

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type All = {
    Common : (* All *) Common
    Home : (* All *) Home
    Link : (* All *) Link
    Item : (* All *) Item
    Settings : (* All *) Settings
    Auth : (* All *) Auth
    About : (* All *) About
    Devices : (* All *) Devices
    Resetting : (* All *) Resetting
    Help : (* All *) Help
    Error : (* All *) Error
} with
    static member Create
        (
            ?common : (* All *) Common,
            ?home : (* All *) Home,
            ?link : (* All *) Link,
            ?item : (* All *) Item,
            ?settings : (* All *) Settings,
            ?auth : (* All *) Auth,
            ?about : (* All *) About,
            ?devices : (* All *) Devices,
            ?resetting : (* All *) Resetting,
            ?help : (* All *) Help,
            ?error : (* All *) Error
        ) : All =
        {
            Common = (* All *) common
                |> Option.defaultWith (fun () -> (Common.Create ()))
            Home = (* All *) home
                |> Option.defaultWith (fun () -> (Home.Create ()))
            Link = (* All *) link
                |> Option.defaultWith (fun () -> (Link.Create ()))
            Item = (* All *) item
                |> Option.defaultWith (fun () -> (Item.Create ()))
            Settings = (* All *) settings
                |> Option.defaultWith (fun () -> (Settings.Create ()))
            Auth = (* All *) auth
                |> Option.defaultWith (fun () -> (Auth.Create ()))
            About = (* All *) about
                |> Option.defaultWith (fun () -> (About.Create ()))
            Devices = (* All *) devices
                |> Option.defaultWith (fun () -> (Devices.Create ()))
            Resetting = (* All *) resetting
                |> Option.defaultWith (fun () -> (Resetting.Create ()))
            Help = (* All *) help
                |> Option.defaultWith (fun () -> (Help.Create ()))
            Error = (* All *) error
                |> Option.defaultWith (fun () -> (Error.Create ()))
        }
    static member SetCommon ((* All *) common : Common) (this : All) =
        {this with Common = common}
    static member SetHome ((* All *) home : Home) (this : All) =
        {this with Home = home}
    static member SetLink ((* All *) link : Link) (this : All) =
        {this with Link = link}
    static member SetItem ((* All *) item : Item) (this : All) =
        {this with Item = item}
    static member SetSettings ((* All *) settings : Settings) (this : All) =
        {this with Settings = settings}
    static member SetAuth ((* All *) auth : Auth) (this : All) =
        {this with Auth = auth}
    static member SetAbout ((* All *) about : About) (this : All) =
        {this with About = about}
    static member SetDevices ((* All *) devices : Devices) (this : All) =
        {this with Devices = devices}
    static member SetResetting ((* All *) resetting : Resetting) (this : All) =
        {this with Resetting = resetting}
    static member SetHelp ((* All *) help : Help) (this : All) =
        {this with Help = help}
    static member SetError ((* All *) error : Error) (this : All) =
        {this with Error = error}
    static member JsonEncoder : JsonEncoder<All> =
        fun (this : All) ->
            E.object [
                "common", Common.JsonEncoder (* All *) this.Common
                "home", Home.JsonEncoder (* All *) this.Home
                "link", Link.JsonEncoder (* All *) this.Link
                "item", Item.JsonEncoder (* All *) this.Item
                "settings", Settings.JsonEncoder (* All *) this.Settings
                "auth", Auth.JsonEncoder (* All *) this.Auth
                "about", About.JsonEncoder (* All *) this.About
                "devices", Devices.JsonEncoder (* All *) this.Devices
                "resetting", Resetting.JsonEncoder (* All *) this.Resetting
                "help", Help.JsonEncoder (* All *) this.Help
                "error", Error.JsonEncoder (* All *) this.Error
            ]
    static member JsonDecoder : JsonDecoder<All> =
        D.object (fun get ->
            {
                Common = get.Optional.Field (* All *) "common" Common.JsonDecoder
                    |> Option.defaultValue (Common.Create ())
                Home = get.Optional.Field (* All *) "home" Home.JsonDecoder
                    |> Option.defaultValue (Home.Create ())
                Link = get.Optional.Field (* All *) "link" Link.JsonDecoder
                    |> Option.defaultValue (Link.Create ())
                Item = get.Optional.Field (* All *) "item" Item.JsonDecoder
                    |> Option.defaultValue (Item.Create ())
                Settings = get.Optional.Field (* All *) "settings" Settings.JsonDecoder
                    |> Option.defaultValue (Settings.Create ())
                Auth = get.Optional.Field (* All *) "auth" Auth.JsonDecoder
                    |> Option.defaultValue (Auth.Create ())
                About = get.Optional.Field (* All *) "about" About.JsonDecoder
                    |> Option.defaultValue (About.Create ())
                Devices = get.Optional.Field (* All *) "devices" Devices.JsonDecoder
                    |> Option.defaultValue (Devices.Create ())
                Resetting = get.Optional.Field (* All *) "resetting" Resetting.JsonDecoder
                    |> Option.defaultValue (Resetting.Create ())
                Help = get.Optional.Field (* All *) "help" Help.JsonDecoder
                    |> Option.defaultValue (Help.Create ())
                Error = get.Optional.Field (* All *) "error" Error.JsonDecoder
                    |> Option.defaultValue (Error.Create ())
            }
        )
    static member JsonSpec =
        FieldSpec.Create<All> (All.JsonEncoder, All.JsonDecoder)
    interface IJson with
        member this.ToJson () = All.JsonEncoder this
    interface IObj
    member this.WithCommon ((* All *) common : Common) =
        this |> All.SetCommon common
    member this.WithHome ((* All *) home : Home) =
        this |> All.SetHome home
    member this.WithLink ((* All *) link : Link) =
        this |> All.SetLink link
    member this.WithItem ((* All *) item : Item) =
        this |> All.SetItem item
    member this.WithSettings ((* All *) settings : Settings) =
        this |> All.SetSettings settings
    member this.WithAuth ((* All *) auth : Auth) =
        this |> All.SetAuth auth
    member this.WithAbout ((* All *) about : About) =
        this |> All.SetAbout about
    member this.WithDevices ((* All *) devices : Devices) =
        this |> All.SetDevices devices
    member this.WithResetting ((* All *) resetting : Resetting) =
        this |> All.SetResetting resetting
    member this.WithHelp ((* All *) help : Help) =
        this |> All.SetHelp help
    member this.WithError ((* All *) error : Error) =
        this |> All.SetError error