[<AutoOpen>]
module SuperClip.Gui.Prefab.Clips

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui
open Dap.Gui.Prefab

[<Literal>]
let ClipsKind = "Clips"

let ClipsJson = parseJson """
{
    "prefab": "clips",
    "styles": [],
    "layout": "full_table",
    "item_prefab": "clip",
    "items": []
}
"""

type ClipsProps = ListProps<ClipProps>

type IClips =
    inherit IListPrefab<ClipsProps, ClipProps>
    inherit IListLayout<ClipsProps, IClip>
    abstract Target : IFullTable with get
    abstract ResizeItems : int -> unit

type Clips (logging : ILogging) =
    inherit WrapList<Clips, ClipsProps, IClip, ClipProps, IFullTable> (ClipsKind, ClipsProps.CreateOf ClipProps.Create, logging)
    do (
        base.Model.AsProperty.LoadJson ClipsJson
    )
    static member Create l = new Clips (l)
    static member Create () = new Clips (getLogging ())
    override this.Self = this
    override __.Spawn l = Clips.Create l
    interface IFallback
    interface IClips with
        member this.Target = this.Target
        member this.ResizeItems size = this.ResizeItems size