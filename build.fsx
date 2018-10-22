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
#load "src/SuperClip.Forms/Dsl.fs"
#load "src/SuperClip.Server/Meta.fs"
#load "src/SuperClip.Server/Dsl.fs"

[<Literal>]
let Dist = "Dist"

let feed =
    NuGet.Feed.Create (
        server = NuGet.ProGet "https://nuget.yjpark.org/nuget/dap",
        apiKey = NuGet.Environment "API_KEY_nuget_yjpark_org"
    )

let libProjects =
    !! "src/SuperClip.Core/*.fsproj"
    ++ "src/SuperClip.Forms/*.fsproj"

let allProjects =
    libProjects
    ++ "src/SuperClip.Ooui/*.fsproj"
    ++ "src/SuperClip.Server/*.fsproj"
    ++ "src/SuperClip.Web/*.fsproj"
    ++ "src/SuperClip.Tools/*.fsproj"

DotNet.create DotNet.release allProjects

DotNet.createPrepares [
    ["SuperClip.Core"], fun _ ->
        SuperClip.Core.Dsl.Compiler.compile ["src" ; "SuperClip.Core"]
        |> List.iter traceSuccess
    ["SuperClip.Forms"], fun _ ->
        SuperClip.Forms.Dsl.compile ["src" ; "SuperClip.Forms"]
        |> List.iter traceSuccess
    ["SuperClip.Server"], fun _ ->
        SuperClip.Server.Dsl.compile ["src" ; "SuperClip.Server"]
        |> List.iter traceSuccess
]

NuGet.extend NuGet.release feed libProjects

Target.runOrDefault DotNet.Build

