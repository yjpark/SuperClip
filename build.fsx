#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.IO.Globbing.Operators

module DotNet = Dap.Build.DotNet

[<Literal>]
let Dist = "Dist"

let allProjects =
    !! "src/SuperClip.Core/*.fsproj"
    ++ "src/SuperClip.App/*.fsproj"
    ++ "src/SuperClip.Ooui/*.fsproj"
    ++ "src/SuperClip.Tools/*.fsproj"

DotNet.createAndRun DotNet.debug allProjects
