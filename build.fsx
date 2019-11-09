(* FAKE: 5.12.1 *)
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
#load "src/SuperClip.Fabulous/Dsl/Text.fs"

[<Literal>]
let Dist = "Dist"

let allProjects =
    !! "src/SuperClip.Core/*.fsproj"
    ++ "src/SuperClip.Server/*.fsproj"
    ++ "src/SuperClip.Web/*.fsproj"
    ++ "src/SuperClip.App/*.fsproj"
    ++ "src/SuperClip.Gui/*.fsproj"
    ++ "src/SuperClip.Fabulous/*.fsproj"
    ++ "src/SuperClip.Ooui/*.fsproj"
    ++ "src/SuperClip.Tools/*.fsproj"

DotNet.create DotNet.debug allProjects

DotNet.createPrepares [
    ["SuperClip.Core"], fun _ ->
        SuperClip.Core.Dsl.Compiler.compile ["src" ; "SuperClip.Core"]
        |> List.iter traceSuccess
    ["SuperClip.App"], fun _ ->
        SuperClip.App.Dsl.compile ["src" ; "SuperClip.App"]
        |> List.iter traceSuccess
    ["SuperClip.Gui"], fun _ ->
        SuperClip.Gui.Dsl.Prefabs.compile ["src" ; "SuperClip.Gui"]
        |> List.iter traceSuccess
    ["SuperClip.Fabulous"], fun _ ->
        SuperClip.Fabulous.Dsl.Text.compile ["src" ; "SuperClip.Fabulous"]
        |> List.iter traceSuccess
    ["SuperClip.Server"], fun _ ->
        SuperClip.Server.Dsl.compile ["src" ; "SuperClip.Server"]
        |> List.iter traceSuccess
]

#load "src/SuperClip.Fabulous/_Gen/Text.fs"
Target.create "Locale.Create" (fun _ ->
    SuperClip.Fabulous.Text.All.Create ()
    |> Dap.Context.Json.encodeJson 4
    |> Dap.Local.TextFile.save "src/SuperClip.App/Assets/Locales/new.json"
    traceSuccess "Locale Created: new.json"
)

Target.create "Locale.Update" (fun _ ->
    Dap.Local.FileSystem.decodeMultiple "src/SuperClip.App/Assets/Locales" SuperClip.Fabulous.Text.All.JsonDecoder
    |> Array.iter (fun (name, text) ->
        text
        |> Dap.Context.Json.encodeJson 4
        |> Dap.Local.TextFile.save (sprintf "src/SuperClip.App/Assets/Locales/%s" name)
        traceSuccess (sprintf "Locale Updated: %s" name)
    )
)

Target.runOrDefault DotNet.Build
