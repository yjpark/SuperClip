module SuperClip.Core.Dsl.Compiler

open Dap.Context.Meta
open Dap.Context.Generator
open Dap.Platform
open Dap.Platform.Meta
open Dap.Platform.Generator

let compile segments =
    [
        Types.compile segments
        Cloud.compile segments
        Packs.compile segments
    ] |> List.concat
