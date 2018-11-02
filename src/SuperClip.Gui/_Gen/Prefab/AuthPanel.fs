[<AutoOpen>]
module SuperClip.Gui.Prefab.AuthPanel

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.Prefab

[<Literal>]
let AuthPanelKind = "AuthPanel"

let AuthPanelJson = parseJson """
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

type AuthPanelProps = StackProps

type IAuthPanel =
    inherit IPrefab<AuthPanelProps>
    abstract Title : ILabel with get
    abstract Name : IInputField with get
    abstract Device : IInputField with get
    abstract Password : IInputField with get
    abstract Cancel : IButton with get
    abstract Auth : IButton with get

type AuthPanel (logging : ILogging) =
    inherit WrapGroup<AuthPanel, AuthPanelProps, IStack> (AuthPanelKind, AuthPanelProps.Create, logging)
    let title : ILabel = base.AsGroup.Add "title" Feature.create<ILabel>
    let name : IInputField = base.AsGroup.Add "name" Feature.create<IInputField>
    let device : IInputField = base.AsGroup.Add "device" Feature.create<IInputField>
    let password : IInputField = base.AsGroup.Add "password" Feature.create<IInputField>
    let cancel : IButton = base.AsGroup.Add "cancel" Feature.create<IButton>
    let auth : IButton = base.AsGroup.Add "auth" Feature.create<IButton>
    do (
        base.Model.AsProperty.LoadJson AuthPanelJson
    )
    static member Create l = new AuthPanel (l)
    static member Create () = new AuthPanel (getLogging ())
    override this.Self = this
    override __.Spawn l = AuthPanel.Create l
    member __.Title : ILabel = title
    member __.Name : IInputField = name
    member __.Device : IInputField = device
    member __.Password : IInputField = password
    member __.Cancel : IButton = cancel
    member __.Auth : IButton = auth
    interface IFallback
    interface IAuthPanel with
        member __.Title : ILabel = title
        member __.Name : IInputField = name
        member __.Device : IInputField = device
        member __.Password : IInputField = password
        member __.Cancel : IButton = cancel
        member __.Auth : IButton = auth