[<AutoOpen>]
[<RequireQualifiedAccess>]
module SuperClip.Prefab.HomeView

open Dap.Prelude
open Dap.Context
open Dap.Gui
open Eto.Forms
open Dap.Eto
open Dap.Eto.Prefab

[<Literal>]
let Kind = "HomeView"

let Json = parseJson """
{
    "prefab": "home_view",
    "styles": [],
    "layout": "vertical_stack",
    "children": {
        "link_status": {
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
        },
        "history": {
            "prefab": "",
            "styles": [],
            "text": "TODO"
        }
    }
}
"""

type Model = Stack.Model
type Widget = Stack.Model

type Prefab (logging : ILogging) =
    inherit Stack.Prefab (logging)
    let linkStatus = LinkStatus.Prefab.AddToGroup logging "link_status" base.Model
    let history = Label.Prefab.AddToGroup logging "history" base.Model
    do (
        base.Model.AsProperty.LoadJson Json
        base.AddChild (linkStatus.Widget)
        base.AddChild (history.Widget)
    )
    static member Create l = new Prefab (l)
    static member Create () = new Prefab (getLogging ())
    static member AddToGroup l key (group : IGroup) =
        let prefab = Prefab.Create l
        group.Children.AddLink<Model> (prefab.Model, key) |> ignore
        prefab
    member __.LinkStatus : LinkStatus.Prefab = linkStatus
    member __.History : Label.Prefab = history