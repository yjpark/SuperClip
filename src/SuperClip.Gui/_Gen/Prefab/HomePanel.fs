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
            "prefab": "clips",
            "styles": [],
            "layout": "full_table",
            "item_prefab": "clip"
        }
    }
}
"""

type HomePanelProps = StackProps

type IHomePanel =
    inherit IPrefab<HomePanelProps>
    abstract Target : IStack with get
    abstract LinkStatus : ILinkStatus with get
    abstract History : IClips with get

type HomePanel (logging : ILogging) =
    inherit WrapCombo<HomePanel, HomePanelProps, IStack> (HomePanelKind, HomePanelProps.Create, logging)
    let linkStatus : ILinkStatus = base.AsComboLayout.Add "link_status" Feature.create<ILinkStatus>
    let history : IClips = base.AsComboLayout.Add "history" Feature.create<IClips>
    do (
        base.Model.AsProperty.LoadJson HomePanelJson
    )
    static member Create l = new HomePanel (l)
    static member Create () = new HomePanel (getLogging ())
    override this.Self = this
    override __.Spawn l = HomePanel.Create l
    member __.LinkStatus : ILinkStatus = linkStatus
    member __.History : IClips = history
    interface IFallback
    interface IHomePanel with
        member this.Target = this.Target
        member __.LinkStatus : ILinkStatus = linkStatus
        member __.History : IClips = history