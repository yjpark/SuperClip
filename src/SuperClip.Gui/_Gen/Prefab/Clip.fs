[<AutoOpen>]
module SuperClip.Gui.Prefab.Clip

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.Prefab

[<Literal>]
let ClipKind = "Clip"

let ClipJson = parseJson """
{
    "prefab": "clip",
    "styles": [],
    "container": "h_box",
    "children": {
        "content": {
            "prefab": "",
            "styles": [],
            "text": "..."
        },
        "delete": {
            "prefab": "",
            "styles": [],
            "disabled": false,
            "text": "Delete"
        },
        "copy": {
            "prefab": "",
            "styles": [],
            "disabled": false,
            "text": "Copy"
        }
    }
}
"""

type ClipProps = ComboProps

type IClip =
    inherit IComboPrefab<ClipProps>
    inherit IGroupPrefab<IHBox>
    abstract Content : ILabel with get
    abstract Delete : IButton with get
    abstract Copy : IButton with get

type Clip (logging : ILogging) =
    inherit BaseCombo<Clip, ClipProps, IHBox> (ClipKind, ClipProps.Create, logging)
    let content : ILabel = base.AsComboPrefab.Add "content" Feature.create<ILabel>
    let delete : IButton = base.AsComboPrefab.Add "delete" Feature.create<IButton>
    let copy : IButton = base.AsComboPrefab.Add "copy" Feature.create<IButton>
    do (
        base.LoadJson' ClipJson
    )
    static member Create l = new Clip (l)
    static member Create () = new Clip (getLogging ())
    override this.Self = this
    override __.Spawn l = Clip.Create l
    member __.Content : ILabel = content
    member __.Delete : IButton = delete
    member __.Copy : IButton = copy
    interface IFallback
    interface IClip with
        member this.Container = this.AsGroupPrefab.Container
        member __.Content : ILabel = content
        member __.Delete : IButton = delete
        member __.Copy : IButton = copy