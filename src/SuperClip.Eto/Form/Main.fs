[<AutoOpen>]
module SuperClip.Eto.Form.Main

open System
open Eto.Forms
open Eto.Drawing

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Eto

open SuperClip.Core
open SuperClip.App
open SuperClip.Prefab

type QuitCommand (app : IApp) =
    inherit Command ()
    do (
        base.MenuText <- "Q&uit"
        base.Shortcut <- Application.Instance.CommonModifier ||| Keys.Q
    )
    override this.OnExecuted (e : EventArgs) =
        base.OnExecuted e
        Application.Instance.Quit()

type MainForm (app : IApp) =
    inherit Form ()
    do (
        base.Title <- "SuperClip"
        base.ClientSize <- Size (640, 800)
        base.Menu <- new MenuBar()
        let fileItem = new ButtonMenuItem(Text = "&File")
        base.Menu.QuitItem <- (new QuitCommand(app)) .CreateMenuItem ()
        let prefab = new HomeView.Prefab (app.Env.Logging)
        base.Content <- prefab.Widget
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

