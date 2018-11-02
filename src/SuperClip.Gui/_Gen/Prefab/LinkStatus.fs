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
}
"""

type LinkStatusProps = StackProps

type ILinkStatus =
    inherit IPrefab<LinkStatusProps>
    abstract Link : ILabel with get
    abstract Session : ILabel with get
    abstract Action : IButton with get

type LinkStatus (logging : ILogging) =
    inherit WrapGroup<LinkStatus, LinkStatusProps, IStack> (LinkStatusKind, LinkStatusProps.Create, logging)
    let link : ILabel = base.AsGroup.Add "link" Feature.create<ILabel>
    let session : ILabel = base.AsGroup.Add "session" Feature.create<ILabel>
    let action : IButton = base.AsGroup.Add "action" Feature.create<IButton>
    do (
        base.Model.AsProperty.LoadJson LinkStatusJson
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
        member __.Link : ILabel = link
        member __.Session : ILabel = session
        member __.Action : IButton = action