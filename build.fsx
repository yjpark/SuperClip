#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.IO.Globbing.Operators

open Dap.Build

#load "src/SuperClip.Core/Dsl.fs"
#load "src/SuperClip.Forms/Dsl.fs"
#load "src/SuperClip.Server/Meta.fs"
#load "src/SuperClip.Server/Dsl.fs"

[<Literal>]
let Dist = "Dist"

let allProjects =
    !! "src/SuperClip.Core/*.fsproj"
    ++ "src/SuperClip.Forms/*.fsproj"
    ++ "src/SuperClip.Ooui/*.fsproj"
    ++ "src/SuperClip.Server/*.fsproj"
    ++ "src/SuperClip.Web/*.fsproj"
    ++ "src/SuperClip.Tools/*.fsproj"

DotNet.create DotNet.debug allProjects

DotNet.createPrepares [
    ["SuperClip.Core"], fun _ ->
        SuperClip.Core.Dsl.compile ["src" ; "SuperClip.Core"]
        |> List.iter traceSuccess
    ["SuperClip.Forms"], fun _ ->
        SuperClip.Forms.Dsl.compile ["src" ; "SuperClip.Forms"]
        |> List.iter traceSuccess
    ["SuperClip.Server"], fun _ ->
        SuperClip.Server.Dsl.compile ["src" ; "SuperClip.Server"]
        |> List.iter traceSuccess
]

Target.runOrDefault DotNet.Build

