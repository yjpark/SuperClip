module SuperClip.Tools.Program

open Argu

open Dap.Prelude
open Dap.Platform

open SuperClip.App
open SuperClip.Tools.Clipboard

type Args =
    | Version
    | [<AltCommandLine("-v")>] Verbose
    | [<CliPrefix(CliPrefix.None)>] ``Watch_Primary`` of ParseResults<WatchPrimary.Args>
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Version -> "Prints the version."
            | Verbose -> "Print a lot of output to stdout."
            | Watch_Primary _ -> "Watch primary clipboard."

let execute (args : ParseResults<Args>) =
    let verbose = args.Contains Verbose
    let consoleLogLevel = if verbose then LogLevelInformation else LogLevelWarning
    Xamarin.Forms.Forms.Init ()
    let app = SuperClip.App.App.initApp consoleLogLevel "super-clip-tools-.log"

    if args.Contains Watch_Primary then
        WatchPrimary.executeAsync app <| args.GetResult Watch_Primary
        |> Util.executeAndWaitForExit app
    else
        raise <| ParseException "no command specified"
    0

[<EntryPoint>]
let main argv =
    let parser = ArgumentParser.Create<Args>(programName = "SuperClip.Tools")
    try
        parser.ParseCommandLine argv
        |> execute
    with
    | ParseException m ->
        let usage = parser.PrintUsage()
        printfn "ERROR: %s\n%s" m usage
        1
    | ExecuteException m as e ->
        printfn "ERROR: %s\n%s" m e.StackTrace
        2
    | e ->
        printfn "%s" e.Message
        1
