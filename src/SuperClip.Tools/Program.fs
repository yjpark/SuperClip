module SuperClip.Tools.Program

open Argu

open Dap.Prelude
open Dap.Platform

open Dap.Prelude
open Dap.Platform

open SuperClip.Core
open SuperClip.App
open SuperClip.Server
open SuperClip.Tools.Misc
open SuperClip.Tools.Clipboard

type Args =
    | Version
    | [<AltCommandLine("-v")>] Verbose
    | [<CliPrefix(CliPrefix.None)>] ``Watch_Primary`` of ParseResults<WatchPrimary.Args>
    | [<CliPrefix(CliPrefix.None)>] ``Init_Db`` of ParseResults<InitDb.Args>
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Version -> "Prints the version."
            | Verbose -> "Print a lot of output to stdout."
            | Watch_Primary _ -> "Watch primary clipboard."
            | Init_Db _ -> "Init Db."

let execute (args : ParseResults<Args>) =
    let verbose = args.Contains Verbose

    (*
    Ooui.UI.Port <- 6061
    Xamarin.Forms.Forms.Init ();
    CrossClipboard.Current <- new ClipboardImplementation ()
    *)

    let consoleLogLevel = if verbose then LogLevelInformation else LogLevelWarning

    if args.Contains Watch_Primary then
        let app = ClientApp.Create ("super-clip-tools-.log", consoleMinLevel = consoleLogLevel)
        WatchPrimary.executeAsync app <| args.GetResult Watch_Primary
        |> Util.executeAndWaitForExit app
    elif args.Contains Init_Db then
        let app = ServerApp.Start ("super-clip-tools-.log", consoleMinLevel = consoleLogLevel)
        InitDb.execute app <| args.GetResult Init_Db
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
