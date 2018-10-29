#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.IO.Globbing.Operators

open Dap.Build

#load "src/SuperClip.Core/Dsl/Types.fs"
#load "src/SuperClip.Core/Dsl/Packs.fs"
#load "src/SuperClip.Core/Dsl/Cloud.fs"
#load "src/SuperClip.Core/Dsl/Compiler.fs"
#load "src/SuperClip.Server/Meta.fs"
#load "src/SuperClip.Server/Dsl.fs"
#load "src/SuperClip.App/Dsl.fs"
#load "src/SuperClip.Gui/Dsl/Prefabs.fs"
#load "src/SuperClip.Eto/Dsl/Prefabs.fs"

[<Literal>]
let Dist = "Dist"

let allProjects =
    !! "src/SuperClip.Server/*.fsproj"
    ++ "src/SuperClip.Web/*.fsproj"
    ++ "src/SuperClip.Gtk/*.fsproj"
    ++ "src/SuperClip.Forms/*.fsproj"
    ++ "src/SuperClip.Ooui/*.fsproj"
    ++ "src/SuperClip.Tools/*.fsproj"

DotNet.create DotNet.debug allProjects

DotNet.createPrepares [
    ["SuperClip.Forms"], fun _ ->
        SuperClip.Core.Dsl.Compiler.compile ["src" ; "SuperClip.Core"]
        |> List.iter traceSuccess
        SuperClip.App.Dsl.compile ["src" ; "SuperClip.App"]
        |> List.iter traceSuccess
        SuperClip.Eto.Dsl.Prefabs.compile ["src" ; "SuperClip.Eto"]
        |> List.iter traceSuccess
    ["SuperClip.Server"], fun _ ->
        SuperClip.Server.Dsl.compile ["src" ; "SuperClip.Server"]
        |> List.iter traceSuccess
]

Target.runOrDefault DotNet.Build
