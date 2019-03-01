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
    "container": "v_box",
    "children": {
        "link_status": {
            "prefab": "link_status",
            "styles": [],
            "container": "h_box",
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
            "container": "table",
            "item_prefab": "clip"
        }
    }
}
"""

type HomePanelProps = ComboProps

type IHomePanel =
    inherit IComboPrefab<HomePanelProps>
    inherit IGroupPrefab<IVBox>
    abstract LinkStatus : ILinkStatus with get
    abstract History : IClips with get

type HomePanel (logging : ILogging) =
    inherit BaseCombo<HomePanel, HomePanelProps, IVBox> (HomePanelKind, HomePanelProps.Create, logging)
    let linkStatus : ILinkStatus = base.AsComboPrefab.Add "link_status" Feature.create<ILinkStatus>
    let history : IClips = base.AsComboPrefab.Add "history" Feature.create<IClips>
    do (
        base.LoadJson' HomePanelJson
    )
    static member Create l = new HomePanel (l)
    static member Create () = new HomePanel (getLogging ())
    override this.Self = this
    override __.Spawn l = HomePanel.Create l
    member __.LinkStatus : ILinkStatus = linkStatus
    member __.History : IClips = history
    interface IFallback
    interface IHomePanel with
        member this.Container = this.AsGroupPrefab.Container
        member __.LinkStatus : ILinkStatus = linkStatus
        member __.History : IClips = history