[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Settings

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Gui
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let render (runner : View) (model : Model) =
    table_view {
        intent TableIntent.Settings
        items [
            (Locale.Text.Settings.SyncSection, [
                switch_cell {
                    text Locale.Text.Settings.CloudMode
                    on (GuiPrefs.getCloudMode runner)
                    onChanged (fun args ->
                        runner |> GuiPrefs.setCloudMode args.Value
                        setupCloudMode runner
                        runner.React <| DoSetPage NoPage
                    )
                }
                text_action_cell {
                    text (GuiPrefs.getAuthChannel runner)
                    detail (GuiPrefs.getAuthDevice runner)
                    action Locale.Text.Settings.ResetAuth
                    onAction (fun _ ->
                        runner |> GuiPrefs.setAuthChannel ""
                        runner |> GuiPrefs.setAuthDevice ""
                        runner.React DoRepaint
                    )
                }
            ])
            (Locale.Text.Settings.DisplaySection, [
                switch_cell {
                    text Locale.Text.Settings.DarkTheme
                    on (Theme.isDark ())
                    onChanged (fun args ->
                        let theme =
                            if args.Value
                                then Theme.DarkTheme
                                else Theme.LightTheme
                        runner |> GuiPrefs.setTheme theme
                        Theme.setDark args.Value
                        runner.React <| DoSetResetting true
                    )
                }
            ])
            (Locale.Text.Settings.LanguageSection,
                IGuiApp.Instance.Locales
                |> Map.toList
                |> List.filter (fun (key, locale) ->
                    not (System.String.IsNullOrEmpty key)
                )|> List.map (fun (key, locale) ->
                    let text', detail', action' = locale.TextForSwitch
                    if IGuiApp.Instance.Locale.Key = key then
                        text_cell {
                            text text'
                            detail detail'
                        }
                    else
                        text_action_cell {
                            text text'
                            detail detail'
                            action action'
                            onAction (fun _ ->
                                runner |> GuiPrefs.setLocale key
                                IGuiApp.Instance.SwitchLocale key
                                runner.React DoRepaint
                            )
                        }
                )
            )
        ]
    }|> contentPage Locale.Text.Settings.Title
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem Locale.Text.Help.Title Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpSettings
            )
        ])
    )