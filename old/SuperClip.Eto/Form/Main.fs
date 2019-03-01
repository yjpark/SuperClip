[<AutoOpen>]
module SuperClip.Eto.Form.Main

open System
open Eto.Forms
open Eto.Drawing

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto
open Dap.Eto.Prefab

open SuperClip.Core
open SuperClip.App

type HomePanel = SuperClip.Gui.Presenter.HomePanel.Presenter

type QuitCommand (app : IApp) =
    inherit Command ()
    do (
        base.MenuText <- "Q&uit"
        base.Shortcut <- Application.Instance.CommonModifier ||| Keys.Q
    )
    override this.OnExecuted (e : EventArgs) =
        base.OnExecuted e
        Application.Instance.Quit()

type MainForm (env : IEnv) =
    inherit Form ()
    let homePanel = new HomePanel (env)
    do (
        base.Title <- "SuperClip"
        base.ClientSize <- Size (640, 800)
        base.Content <- homePanel.Prefab.Widget0 :?> StackWidget
        (*
        base.Menu <- new MenuBar()
        let fileItem = new ButtonMenuItem(Text = "&File")
        base.Menu.QuitItem <- (new QuitCommand(env)) .CreateMenuItem ()
        *)
        // about command (goes in Application menu on OS X, Help menu for others)
        (*
        base.Menu.AboutItem <- (new Command((fun sender e -> (new Dialog(Content = (new Label(Text = "About my app...")), ClientSize = Size(200, 200))).ShowModal(base)), MenuText = "About my app")).CreateMenuItem()
        *)
        // create toolbar
        (*
        base.ToolBar <- new ToolBar()
        base.ToolBar.Items.Add(new OpenCommand(env))
        base.ToolBar.Items.Add(new SeparatorToolItem())
        let prefab = new LoginForm.Prefab (env.Logging)
        logWarn env "JSON" "NEW" <| encodeJson 4 prefab
        base.Content <- prefab.Widget
        prefab.Password.Value.Model.Text.OnChanged.AddWatcher env "Test" (fun evt ->
            logWarn env "Password" "Changed" (evt.Old, evt.New)
        )
        prefab.Login.OnClick.OnEvent.AddWatcher env "Test" (fun evt ->
            logWarn env "Login" "OnClick" evt
        )
        prefab.Password.Value.Model.Text.SetValue "KK"
        prefab.Title.Model.Text.SetValue "Hello World"
        *)
    )
    member __.Attach (app : IApp) =
        homePanel.Attach app

