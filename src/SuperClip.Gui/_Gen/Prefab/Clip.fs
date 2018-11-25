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
    "layout": "vertical_stack",
    "children": {
        "content": {
            "prefab": "",
            "styles": [],
            "text": "..."
        },
        "delete": {
            "prefab": "",
            "styles": [],
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

type ClipProps = StackProps

type IClip =
    inherit IPrefab<ClipProps>
    abstract Content : ILabel with get
    abstract Delete : ILabel with get
    abstract Copy : IButton with get

type Clip (logging : ILogging) =
    inherit WrapCombo<Clip, ClipProps, IStack> (ClipKind, ClipProps.Create, logging)
    let content : ILabel = base.AsComboLayout.Add "content" Feature.create<ILabel>
    let delete : ILabel = base.AsComboLayout.Add "delete" Feature.create<ILabel>
    let copy : IButton = base.AsComboLayout.Add "copy" Feature.create<IButton>
    do (
        base.Model.AsProperty.LoadJson ClipJson
    )
    static member Create l = new Clip (l)
    static member Create () = new Clip (getLogging ())
    override this.Self = this
    override __.Spawn l = Clip.Create l
    member __.Content : ILabel = content
    member __.Delete : ILabel = delete
    member __.Copy : IButton = copy
    interface IFallback
    interface IClip with
        member __.Content : ILabel = content
        member __.Delete : ILabel = delete
        member __.Copy : IButton = copy