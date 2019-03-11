[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Theme

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Decorator
open Dap.Fabulous.Palette

[<Literal>]
let LightTheme = "Light"

[<Literal>]
let DarkTheme = "Dark"

[<Literal>]
let TextCell_Current = "text_cell_current"

[<Literal>]
let Label_Dimmed = "label_dimmed"

[<Literal>]
let Label_Linked = "label_linked"

[<Literal>]
let Label_Linking = "label_linking"

[<Literal>]
let Label_NoLink = "label_no_link"

[<Literal>]
let Button_Big = "button_big"

type SuperClipColorScheme = {
    Fabulous : ColorScheme
    Current : Color
    Linked : Color
    Linking : Color
    NoLink : Color
}

let lightParam : SuperClipColorScheme = {
    Fabulous = Material.LightScheme
    Current = Material.Palettes.Blue.Normal600
    Linked = Material.Palettes.Green.Normal600
    Linking = Material.Palettes.Orange.Normal900
    NoLink = Material.Palettes.Red.Normal500
}

let darkParam : SuperClipColorScheme = {
    Fabulous = Material.DarkScheme
    Current = Material.Palettes.Blue.Normal500
    Linked = Material.Palettes.Green.Normal500
    Linking = Material.Palettes.Orange.Normal700
    NoLink = Material.Palettes.Red.Normal300
}

let private setup (theme : ITheme) (param : SuperClipColorScheme) =
    theme.AddFabulousColorScheme param.Fabulous
    theme.AddDecorator TextCell_Current
        (new TextCell.Decorator
            (textColor = param.Current, ?detailColor = param.Fabulous.Label.Dimmed))
    theme.AddDecorator Label_Dimmed
        (new Label.Decorator
            (?textColor = param.Fabulous.Label.Dimmed))
    theme.AddDecorator Label_Linked
        (new Label.Decorator
            (textColor = param.Linked))
    theme.AddDecorator Label_Linking
        (new Label.Decorator
            (textColor = param.Linking))
    theme.AddDecorator Label_NoLink
        (new Label.Decorator
            (textColor = param.NoLink))
    theme.AddDecorator Button_Big
        (new Button.Decorator
            (?backgroundColor = param.Fabulous.Panel.Surface))

type ThemeHook (logging : ILogging) =
    inherit EmptyContext(logging, "ThemeHook")
    interface IGuiAppHook with
        member __.Init (app : IGuiApp) =
            app.AddTheme LightTheme lightParam setup
            app.AddTheme DarkTheme darkParam setup

let isDark () : bool =
    IGuiApp.Instance.Theme.Key = DarkTheme

let setDark (dark : bool) =
    if dark then
        IGuiApp.Instance.SwitchTheme DarkTheme
    else
        IGuiApp.Instance.SwitchTheme LightTheme

let decorate<'widget when 'widget :> Element> (widget : 'widget) =
    IGuiApp.Instance.Theme.DecorateFabulous<'widget> widget

let getValue (get : SuperClipColorScheme -> 'v) =
    IGuiApp.Instance.Theme.Param
    :?> SuperClipColorScheme
    |> get