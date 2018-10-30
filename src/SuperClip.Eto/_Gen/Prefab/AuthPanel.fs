[<RequireQualifiedAccess>]
module SuperClip.Prefab.AuthPanel

open Dap.Prelude
open Dap.Context
open Dap.Gui
open Eto.Forms
open Dap.Eto
open Dap.Eto.Prefab

[<Literal>]
let Kind = "AuthPanel"

let Json = parseJson """
{
    "prefab": "auth_panel",
    "styles": [
        "style1",
        "style2"
    ],
    "layout": "vertical_stack",
    "children": {
        "title": {
            "prefab": "",
            "styles": [],
            "text": "Please Provide Authentication Details"
        },
        "name": {
            "prefab": "input_field",
            "styles": [
                "style3"
            ],
            "layout": "horizontal_stack",
            "children": {
                "label": {
                    "prefab": "",
                    "styles": [],
                    "text": "User Name"
                },
                "value": {
                    "prefab": "",
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "device": {
            "prefab": "input_field",
            "styles": [
                "style3"
            ],
            "layout": "horizontal_stack",
            "children": {
                "label": {
                    "prefab": "",
                    "styles": [],
                    "text": "Device Name"
                },
                "value": {
                    "prefab": "",
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "password": {
            "prefab": "input_field",
            "styles": [
                "style3"
            ],
            "layout": "horizontal_stack",
            "children": {
                "label": {
                    "prefab": "",
                    "styles": [],
                    "text": "Password"
                },
                "value": {
                    "prefab": "",
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "cancel": {
            "prefab": "",
            "styles": [],
            "disabled": false,
            "text": "Cancel"
        },
        "auth": {
            "prefab": "",
            "styles": [],
            "disabled": false,
            "text": "Auth"
        }
    }
}
"""

type Model = Stack.Model
type Widget = Stack.Model

type Prefab (logging : ILogging) =
    inherit Stack.Prefab (logging)
    let title = Label.Prefab.AddToGroup logging "title" base.Model
    let name = InputField.Prefab.AddToGroup logging "name" base.Model
    let device = InputField.Prefab.AddToGroup logging "device" base.Model
    let password = InputField.Prefab.AddToGroup logging "password" base.Model
    let cancel = Button.Prefab.AddToGroup logging "cancel" base.Model
    let auth = Button.Prefab.AddToGroup logging "auth" base.Model
    do (
        base.Model.AsProperty.LoadJson Json
        base.AddChild (title.Widget)
        base.AddChild (name.Widget)
        base.AddChild (device.Widget)
        base.AddChild (password.Widget)
        base.AddChild (cancel.Widget)
        base.AddChild (auth.Widget)
    )
    static member Create l = new Prefab (l)
    static member Create () = new Prefab (getLogging ())
    static member AddToGroup l key (group : IGroup) =
        let prefab = Prefab.Create l
        group.Children.AddLink<Model> (prefab.Model, key) |> ignore
        prefab
    member __.Title : Label.Prefab = title
    member __.Name : InputField.Prefab = name
    member __.Device : InputField.Prefab = device
    member __.Password : InputField.Prefab = password
    member __.Cancel : Button.Prefab = cancel
    member __.Auth : Button.Prefab = auth