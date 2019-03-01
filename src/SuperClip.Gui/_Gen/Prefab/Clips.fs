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
    "container": "table",
    "item_prefab": "clip",
    "items": []
}
"""

type ClipsProps = ListProps

type IClips =
    inherit IListPrefab<ClipsProps, IClip>
    inherit IGroupPrefab<ITable>

type Clips (logging : ILogging) =
    inherit BaseList<Clips, ClipsProps, IClip, ClipProps, ITable> (ClipsKind, ClipsProps.Create, logging)
    do (
        base.LoadJson' ClipsJson
    )
    static member Create l = new Clips (l)
    static member Create () = new Clips (getLogging ())
    override this.Self = this
    override __.Spawn l = Clips.Create l
    interface IFallback
    interface IClips with
        member this.Container = this.AsGroupPrefab.Container