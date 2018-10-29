[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Prefab.LinkStatus

open Dap.Prelude
open Dap.Context
open Dap.Gui
open Eto.Forms
open Dap.Eto
open Dap.Eto.Prefab

[<Literal>]
let Kind = "LinkStatus"

let Json = parseJson """
{
    "prefab": "link_status",
    "styles": [],
    "layout": "horizontal_stack",
    "children": {
        "status": {
            "prefab": "",
            "styles": [],
            "text": "..."
        },
        "action": {
            "prefab": "",
            "styles": [],
            "disabled": false,
            "text": "Action"
        }
    }
}
"""

type Model = Stack.Model
type Widget = Stack.Model

type Prefab (logging : ILogging) =
    inherit Stack.Prefab (logging)
    let status = Label.Prefab.AddToGroup logging "status" base.Model
    let action = Button.Prefab.AddToGroup logging "action" base.Model
    do (
        base.Model.AsProperty.LoadJson Json
        base.AddChild (status.Widget)
        base.AddChild (action.Widget)
    )
    static member Create l = new Prefab (l)
    static member Create () = new Prefab (getLogging ())
    static member AddToGroup l key (group : IGroup) =
        let prefab = Prefab.Create l
        group.Children.AddLink<Model> (prefab.Model, key) |> ignore
        prefab
    member __.Status : Label.Prefab = status
    member __.Action : Button.Prefab = action