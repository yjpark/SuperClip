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

let init () =
    Theme.create LightTheme lightParam setup
    Theme.create DarkTheme darkParam setup

let isDark () : bool =
    (Theme.get None) .Key = DarkTheme

let setDark (dark : bool) =
    if dark then
        Theme.switch (DarkTheme)
    else
        Theme.switch (LightTheme)

let decorate<'widget when 'widget :> Element> (widget : 'widget) =
    (Theme.get None) .DecorateFabulous<'widget> widget

let getValue (get : SuperClipColorScheme -> 'v) =
    ((Theme.get None) .Param)
    :?> SuperClipColorScheme
    |> get