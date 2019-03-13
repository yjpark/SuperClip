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
let TextCell_Current = "TextCell_Current"

[<Literal>]
let TextActionCell_Linked = "TextActionCell_Linked"

[<Literal>]
let TextActionCell_Linking = "TextActionCell_Linking"

[<Literal>]
let TextActionCell_NoLink = "TextActionCell_NoLink"

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
            app.AddTheme DarkTheme darkParam setup
            app.AddTheme LightTheme lightParam setup

let isDark () : bool =
    IGuiApp.Instance.Theme.Key = DarkTheme

let setDark (dark : bool) =
    if dark then
        IGuiApp.Instance.SwitchTheme DarkTheme
    else
        IGuiApp.Instance.SwitchTheme LightTheme

let getValue (get : SuperClipColorScheme -> 'v) =
    IGuiApp.Instance.Theme.Param
    :?> SuperClipColorScheme
    |> get