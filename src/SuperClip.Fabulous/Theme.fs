[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Theme

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Platform
open Dap.Gui
open Dap.Fabulous
open Dap.Fabulous.Decorator
open Dap.Fabulous.Theme

[<Literal>]
let TextCell_Current = "text_cell_current"

type SuperClipColors = {
    Current : Color
    Linked : Color
    Linking : Color
    NoLink : Color
}

type SuperClipTheme = {
    Fabulous : FabulousTheme
    Colors : SuperClipColors
}

type Theme with
    member this.WithSuperClip (param : SuperClipTheme) =
        this.WithFabulous param.Fabulous |> ignore
        this.AddDecorator TextCell_Current
            (new TextCell.Decorator (textColor = param.Colors.Current, detailColor = param.Fabulous.Colors.Secondary))
        this

let getSuperClipTheme (param : SuperClipTheme) =
    (new Theme ()) .WithSuperClip param

let lightParam : SuperClipTheme = {
    Fabulous = Light.param
    Colors = {
        Current = Color.Blue
        Linked = Color.Green
        Linking = Color.Yellow
        NoLink = Color.Red
    }
}

let darkParam : SuperClipTheme = {
    Fabulous = Dark.param
    Colors = {
        Current = Color.Blue
        Linked = Color.Green
        Linking = Color.Yellow
        NoLink = Color.Red
    }
}

let lightTheme : Theme = getSuperClipTheme lightParam
let darkTheme : Theme = getSuperClipTheme darkParam

let mutable private theme : Theme = darkTheme

let isDark () =
    theme = darkTheme

let setDark (dark : bool) =
    theme <-
        if dark then
            darkTheme
        else
            lightTheme

let decorate<'widget when 'widget :> Element> (widget : 'widget) =
    theme.DecorateFabulous<'widget> widget

let getValue (get : SuperClipTheme -> 'v) =
    if isDark () then
        darkParam
    else
        lightParam
    |> get