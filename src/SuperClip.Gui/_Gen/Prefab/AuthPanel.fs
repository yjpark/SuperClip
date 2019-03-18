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
    "theme": null,
    "styles": [
        "style1",
        "style2"
    ],
    "container": "v_box",
    "children": {
        "title": {
            "prefab": "",
            "theme": null,
            "styles": [],
            "text": "Please Provide Authentication Details"
        },
        "name": {
            "prefab": "input_field",
            "theme": null,
            "styles": [
                "style3"
            ],
            "container": "h_box",
            "children": {
                "label": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "text": "User Name"
                },
                "value": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "device": {
            "prefab": "input_field",
            "theme": null,
            "styles": [
                "style3"
            ],
            "container": "h_box",
            "children": {
                "label": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "text": "Device Name"
                },
                "value": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "password": {
            "prefab": "input_field",
            "theme": null,
            "styles": [
                "style3"
            ],
            "container": "h_box",
            "children": {
                "label": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "text": "Password"
                },
                "value": {
                    "prefab": "",
                    "theme": null,
                    "styles": [],
                    "disabled": false,
                    "text": ""
                }
            }
        },
        "cancel": {
            "prefab": "",
            "theme": null,
            "styles": [],
            "disabled": false,
            "text": "Cancel"
        },
        "auth": {
            "prefab": "",
            "theme": null,
            "styles": [],
            "disabled": false,
            "text": "Auth"
        }
    }
}
"""

type AuthPanelProps = ComboProps

type IAuthPanel =
    inherit IComboPrefab<AuthPanelProps>
    inherit IGroupPrefab<IVBox>
    abstract Title : ILabel with get
    abstract Name : IInputField with get
    abstract Device : IInputField with get
    abstract Password : IInputField with get
    abstract Cancel : IButton with get
    abstract Auth : IButton with get

type AuthPanel (logging : ILogging) =
    inherit BaseCombo<AuthPanel, AuthPanelProps, IVBox> (AuthPanelKind, AuthPanelProps.Create, logging)
    let title : ILabel = base.AsComboPrefab.Add "title" Feature.create<ILabel>
    let name : IInputField = base.AsComboPrefab.Add "name" Feature.create<IInputField>
    let device : IInputField = base.AsComboPrefab.Add "device" Feature.create<IInputField>
    let password : IInputField = base.AsComboPrefab.Add "password" Feature.create<IInputField>
    let cancel : IButton = base.AsComboPrefab.Add "cancel" Feature.create<IButton>
    let auth : IButton = base.AsComboPrefab.Add "auth" Feature.create<IButton>
    do (
        base.LoadJson' AuthPanelJson
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
        member this.Container = this.AsGroupPrefab.Container
        member __.Title : ILabel = title
        member __.Name : IInputField = name
        member __.Device : IInputField = device
        member __.Password : IInputField = password
        member __.Cancel : IButton = cancel
        member __.Auth : IButton = auth