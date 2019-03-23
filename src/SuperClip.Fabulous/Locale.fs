[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Locale

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Local
open Dap.Gui

open SuperClip.App

let DefaultLocale = "en"

type ILocale with
    member this.Param = this.Param0 :?> Text.All

type Locale = SuperClipLocale
with
    static member Text = IGuiApp.Instance.Locale.Param

type LocaleHook (logging : ILogging) =
    inherit EmptyContext(logging, "LocaleHook")
    interface IGuiAppHook with
        member __.OnInit (app : IGuiApp) =
            let assembly = getSuperClipAppAssembly ()
            EmbeddedResource.LogNames (app, assembly)
            let text = Text.All.Create ()
            app.AddDefaultLocale text
            app.LoadLocalesFromEmbeddedResource ("Assets/Locales/", Text.All.JsonDecoder, assembly)
            app.SwitchLanguage ("en")
            //app.SwitchRegion ("zh", "CN")