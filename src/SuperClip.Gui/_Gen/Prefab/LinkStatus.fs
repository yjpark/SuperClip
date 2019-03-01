[<AutoOpen>]
module SuperClip.Gui.Prefab.LinkStatus

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.Prefab

[<Literal>]
let LinkStatusKind = "LinkStatus"

let LinkStatusJson = parseJson """
{
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
}
"""

type LinkStatusProps = ComboProps

type ILinkStatus =
    inherit IComboPrefab<LinkStatusProps>
    inherit IGroupPrefab<IHBox>
    abstract Link : ILabel with get
    abstract Session : ILabel with get
    abstract Action : IButton with get

type LinkStatus (logging : ILogging) =
    inherit BaseCombo<LinkStatus, LinkStatusProps, IHBox> (LinkStatusKind, LinkStatusProps.Create, logging)
    let link : ILabel = base.AsComboPrefab.Add "link" Feature.create<ILabel>
    let session : ILabel = base.AsComboPrefab.Add "session" Feature.create<ILabel>
    let action : IButton = base.AsComboPrefab.Add "action" Feature.create<IButton>
    do (
        base.LoadJson' LinkStatusJson
    )
    static member Create l = new LinkStatus (l)
    static member Create () = new LinkStatus (getLogging ())
    override this.Self = this
    override __.Spawn l = LinkStatus.Create l
    member __.Link : ILabel = link
    member __.Session : ILabel = session
    member __.Action : IButton = action
    interface IFallback
    interface ILinkStatus with
        member this.Container = this.AsGroupPrefab.Container
        member __.Link : ILabel = link
        member __.Session : ILabel = session
        member __.Action : IButton = action