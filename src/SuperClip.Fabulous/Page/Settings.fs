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
                    on (GuiPrefs.getCloudMode ())
                    onChanged (fun args ->
                        GuiPrefs.setCloudMode args.Value
                        setupCloudMode runner
                        runner.React <| DoSetPage NoPage
                    )
                }
                switch_cell {
                    text Locale.Text.Settings.SeparateSyncing
                    on (GuiPrefs.getSeparateSyncing ())
                    onChanged (fun args ->
                        GuiPrefs.setSeparateSyncing args.Value
                        setupSeparateSyncing runner
                        runner.Pack.Session.Actor.State.Channel
                        |> Option.iter (fun _ ->
                            runner.React <| DoSetPage NoPage
                        )
                    )
                }
            ])
            (Locale.Text.Settings.ServerSection, [
                match model.EditingServerUri with
                | Some serverUri ->
                    yield text_action_cell {
                        text (GuiPrefs.getCloudServerUri ())
                        action Locale.Text.Settings.Cancel
                        onAction (fun _ ->
                            model.EditingServerUri <- None
                            runner.React DoRepaint
                        )
                    }
                    yield entry_cell {
                        text serverUri
                        textChanged (fun args ->
                            model.EditingServerUri <- Some args.NewTextValue
                        )
                    }
                    yield text_action_cell {
                        action Locale.Text.Settings.Done
                        onAction (fun _ ->
                            GuiPrefs.setCloudServerUri model.EditingServerUri.Value
                            model.EditingServerUri <- None
                            setupCloudServerUri true runner
                            runner.React <| DoSetPage NoPage
                        )
                    }
                | None ->
                    let isDefaultUri = GuiPrefs.isDefaultCloudServerUri ()
                    yield text_action_cell {
                        text (GuiPrefs.getCloudServerUri ())
                        action (if isDefaultUri then Locale.Text.Settings.ChangeServerUri else Locale.Text.Settings.Reset)
                        onAction (fun _ ->
                            if isDefaultUri then
                                model.EditingServerUri <- Some ""
                                runner.React DoRepaint
                            else
                                GuiPrefs.setCloudServerUri ""
                                setupCloudServerUri true runner
                                runner.React <| DoSetPage NoPage
                        )
                    }
            ])
            (Locale.Text.Settings.AuthSection, [
                text_action_cell {
                    text (GuiPrefs.getAuthChannel ())
                    detail (GuiPrefs.getAuthDevice ())
                    action Locale.Text.Settings.Reset
                    onAction (fun _ ->
                        GuiPrefs.setAuthChannel ""
                        GuiPrefs.setAuthDevice ""
                        runner.React DoRepaint
                        runner.Pack.Session.Actor.State.Auth
                        |> Option.iter (fun _ ->
                            runner.Pack.Session.Post <| SessionTypes.DoResetAuth None
                            runner.React <| DoSetPage NoPage
                        )
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
                        GuiPrefs.setTheme theme
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
                                GuiPrefs.setLocale key
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