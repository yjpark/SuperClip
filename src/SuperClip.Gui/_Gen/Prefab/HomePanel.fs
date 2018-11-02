[<AutoOpen>]
module SuperClip.Gui.Prefab.HomePanel

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.Prefab

[<Literal>]
let HomePanelKind = "HomePanel"

let HomePanelJson = parseJson """
{
    "prefab": "home_panel",
    "styles": [],
    "layout": "vertical_stack",
    "children": {
        "link_status": {
            "prefab": "link_status",
            "styles": [],
            "layout": "horizontal_stack",
            "children": {
                "link": {
                    "prefab": "",
                    "styles": [],
                    "text": "..."
                },
                "session": {
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

type HomePanelProps = StackProps

type IHomePanel =
    inherit IPrefab<HomePanelProps>
    abstract LinkStatus : ILinkStatus with get
    abstract History : ILabel with get

type HomePanel (logging : ILogging) =
    inherit WrapGroup<HomePanel, HomePanelProps, IStack> (HomePanelKind, HomePanelProps.Create, logging)
    let linkStatus : ILinkStatus = base.AsGroup.Add "link_status" Feature.create<ILinkStatus>
    let history : ILabel = base.AsGroup.Add "history" Feature.create<ILabel>
    do (
        base.Model.AsProperty.LoadJson HomePanelJson
    )
    static member Create l = new HomePanel (l)
    static member Create () = new HomePanel (getLogging ())
    override this.Self = this
    override __.Spawn l = HomePanel.Create l
    member __.LinkStatus : ILinkStatus = linkStatus
    member __.History : ILabel = history
    interface IFallback
    interface IHomePanel with
        member __.LinkStatus : ILinkStatus = linkStatus
        member __.History : ILabel = history