[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Theme

open SkiaSharp
open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Skia
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Decorator
open Dap.Fabulous.Palette

[<Literal>]
let LightTheme = "Light"

[<Literal>]
let DarkTheme = "Dark"

[<Literal>]
let TextCell_Current = "TextCell_Current"

[<Literal>]
let TextActionCell_Linked = "TextActionCell_Linked"

[<Literal>]
let TextActionCell_Linking = "TextActionCell_Linking"

[<Literal>]
let TextActionCell_NoLink = "TextActionCell_NoLink"

[<Literal>]
let Button_Big = "Button_Big"

type Icons (folder : string, color : SKColor) =
    inherit IoniconsCache (folder, [ Icons.Settings ; Icons.Help ], color, Icons.Size)
    static member Size = 128
    static member Settings = Ionicons.MD.Settings
    static member Help = Ionicons.MD.Help

let icons = new Icons ("icons", SKColors.Black)

type SuperClipThemeParam = {
    Fabulous : ColorScheme
    Current : Color
    Linked : Color
    Linking : Color
    NoLink : Color
}

type ITheme with
    member this.Param = this.Param0 :?> SuperClipThemeParam

let lightParam : SuperClipThemeParam = {
    Fabulous = Material.LightScheme
    Current = Material.Palettes.Blue.Normal600
    Linked = Material.Palettes.Green.Normal600
    Linking = Material.Palettes.Orange.Normal900
    NoLink = Material.Palettes.Red.Normal500
}

let darkParam : SuperClipThemeParam = {
    Fabulous = Material.DarkScheme
    Current = Material.Palettes.Blue.Normal500
    Linked = Material.Palettes.Green.Normal500
    Linking = Material.Palettes.Orange.Normal700
    NoLink = Material.Palettes.Red.Normal300
}

let private setup (theme : ITheme) (param : SuperClipThemeParam) =
    theme.AddFabulousColorScheme param.Fabulous
    theme.AddDecorator TextCell_Current
        (new TextCell.Decorator
            (textColor = param.Current, ?detailColor = param.Fabulous.Label.Dimmed))
    theme.AddDecorator TextActionCell_Linked
        (new TextActionCell.Decorator
            (textColor = param.Linked))
    theme.AddDecorator TextActionCell_Linking
        (new TextActionCell.Decorator
            (textColor = param.Linking))
    theme.AddDecorator TextActionCell_NoLink
        (new TextActionCell.Decorator
            (textColor = param.NoLink))
    theme.AddDecorator Button_Big
        (new Button.Decorator
            (?backgroundColor = param.Fabulous.Panel.Surface, update = (fun button ->
                button.HorizontalOptions <- LayoutOptions.FillAndExpand
                button.VerticalOptions <- LayoutOptions.Center
            )))

type ThemeHook (logging : ILogging) =
    inherit EmptyContext(logging, "ThemeHook")
    interface IGuiAppHook with
        member __.OnInit (app : IGuiApp) =
            app.AddTheme LightTheme lightParam setup
            app.AddTheme DarkTheme darkParam setup
            //TODO Switch to Dark if needed

let isDark () : bool =
    IGuiApp.Instance.Theme.Key = DarkTheme

let setDark (dark : bool) =
    if dark then
        IGuiApp.Instance.SwitchTheme DarkTheme
    else
        IGuiApp.Instance.SwitchTheme LightTheme
