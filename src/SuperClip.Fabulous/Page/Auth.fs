[<RequireQualifiedAccess>]
module SuperClip.Fabulous.Page.Auth

open Xamarin.Forms
open Fabulous.DynamicViews

open Dap.Prelude
open Dap.Platform
open Dap.Remote
open Dap.Fabulous.Builder

open SuperClip.Core
open SuperClip.App
open SuperClip.Fabulous
open SuperClip.Fabulous.View.Types

module HistoryTypes = SuperClip.Core.History.Types
module SessionTypes = SuperClip.App.Session.Types

let doAuth (runner : View) (model : Model) : unit =
    let password = model.Password
    let auth : Credential =
        {
            Device = Device.New (GuiPrefs.getAuthDevice runner)
            Channel = Channel.CreateWithName (GuiPrefs.getAuthChannel runner)
            PassHash = calcPassHash password
            CryptoKey = calcCryptoKey password
            Token = ""
        }
    runner.Pack.Session.Post <| SessionTypes.DoSetAuth (auth, None)
    model.Password <- ""

let render (runner : View) (model : Model) : Widget =
    v_box {
        children [
            label {
                text Locale.Text.Auth.Channel
            }
            entry {
                text (GuiPrefs.getAuthChannel runner)
                keyboard Keyboard.Email
                textChanged (fun args ->
                    GuiPrefs.setAuthChannel args.NewTextValue runner
                )
                completed (fun text ->
                    GuiPrefs.setAuthChannel text runner
                )
            }
            label {
                text Locale.Text.Auth.Device
            }
            entry {
                text (GuiPrefs.getAuthDevice runner)
                textChanged (fun args ->
                    //Can NOT send msg here, which will mess with on screen keyboard
                    GuiPrefs.setAuthDevice args.NewTextValue runner
                )
                completed (fun text ->
                    GuiPrefs.setAuthDevice text runner
                )
            }
            label {
                text Locale.Text.Auth.Password
            }
            entry {
                text ""
                isPassword true
                textChanged (fun args ->
                    model.Password <- args.NewTextValue
                )
                completed (fun text ->
                    model.Password <- text
                )
            }
            label {
                text ""
            }
            button {
                classId Theme.Button_Big
                text Locale.Text.Auth.Login
                canExecute (not model.LoggingIn)
                command (fun () ->
                    if GuiPrefs.getAuthChannel runner = ""
                        || GuiPrefs.getAuthDevice runner = ""
                        || model.Password = "" then
                        ("Error", "Please Fill in All Fields", None)
                        |> setInfoDialog runner "Auth Failed"
                    else
                        doAuth runner model
                        runner.React <| DoSetLoggingIn true
                )
            }
        ]
    }|> scrollPage Locale.Text.Auth.Title
    |> (fun view ->
        view.ToolbarItems([
            yield toolbarItem Locale.Text.Help.Title Icons.Help (fun () ->
                runner.React <| DoSetHelp ^<| Some HelpAuth
            )
        ])
    )
