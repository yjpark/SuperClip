module SuperClip.Core.CoreApp

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Dap.Prelude
open Dap.Context
open Dap.Context.Builder
open Dap.Platform

module TickerTypes = Dap.Platform.Ticker.Types
module PrimaryTypes = SuperClip.Core.Primary.Types
module HistoryTypes = SuperClip.Core.History.Types

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
type CoreAppArgs = {
    Ticker : (* IServicesPack *) TickerTypes.Args
    PrimaryClipboard : (* ICorePack *) PrimaryTypes.Args
    LocalHistory : (* ICorePack *) HistoryTypes.Args
    History : (* ICorePack *) HistoryTypes.Args
} with
    static member Create ticker primaryClipboard localHistory history
            : CoreAppArgs =
        {
            Ticker = ticker
            PrimaryClipboard = primaryClipboard
            LocalHistory = localHistory
            History = history
        }
    static member Default () =
        CoreAppArgs.Create
            (TickerTypes.Args.Default ())
            (PrimaryTypes.Args.Default ())
            (HistoryTypes.Args.Default ())
            (HistoryTypes.Args.Default ())
    static member JsonEncoder : JsonEncoder<CoreAppArgs> =
        fun (this : CoreAppArgs) ->
            E.object [
                "ticker", TickerTypes.Args.JsonEncoder this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder this.History
            ]
    static member JsonDecoder : JsonDecoder<CoreAppArgs> =
        D.decode CoreAppArgs.Create
        |> D.optional "ticker" TickerTypes.Args.JsonDecoder (TickerTypes.Args.Default ())
        |> D.optional "primary_clipboard" PrimaryTypes.Args.JsonDecoder (PrimaryTypes.Args.Default ())
        |> D.optional "local_history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
        |> D.optional "history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
    static member JsonSpec =
        FieldSpec.Create<CoreAppArgs>
            CoreAppArgs.JsonEncoder CoreAppArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = CoreAppArgs.JsonEncoder this
    interface IObj
    member this.WithTicker ((* IServicesPack *) ticker : TickerTypes.Args) = {this with Ticker = ticker}
    member this.WithPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) = {this with PrimaryClipboard = primaryClipboard}
    member this.WithLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) = {this with LocalHistory = localHistory}
    member this.WithHistory ((* ICorePack *) history : HistoryTypes.Args) = {this with History = history}
    interface ICorePackArgs with
        member this.PrimaryClipboard (* ICorePack *) : PrimaryTypes.Args = this.PrimaryClipboard
        member this.LocalHistory (* ICorePack *) : HistoryTypes.Args = this.LocalHistory
        member this.History (* ICorePack *) : HistoryTypes.Args = this.History
    interface IServicesPackArgs with
        member this.Ticker (* IServicesPack *) : TickerTypes.Args = this.Ticker
    member this.AsServicesPackArgs = this :> IServicesPackArgs
    member this.AsCorePackArgs = this :> ICorePackArgs

(*
 * Generated: <ValueBuilder>
 *)
type CoreAppArgsBuilder () =
    inherit ObjBuilder<CoreAppArgs> ()
    override __.Zero () = CoreAppArgs.Default ()
    [<CustomOperation("ticker")>]
    member __.Ticker (target : CoreAppArgs, (* IServicesPack *) ticker : TickerTypes.Args) =
        target.WithTicker ticker
    [<CustomOperation("primary_clipboard")>]
    member __.PrimaryClipboard (target : CoreAppArgs, (* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        target.WithPrimaryClipboard primaryClipboard
    [<CustomOperation("local_history")>]
    member __.LocalHistory (target : CoreAppArgs, (* ICorePack *) localHistory : HistoryTypes.Args) =
        target.WithLocalHistory localHistory
    [<CustomOperation("history")>]
    member __.History (target : CoreAppArgs, (* ICorePack *) history : HistoryTypes.Args) =
        target.WithHistory history

let coreAppArgs = CoreAppArgsBuilder ()

type ICoreApp =
    inherit IPack
    inherit ICorePack
    abstract Args : CoreAppArgs with get

type CoreApp (loggingArgs : LoggingArgs, scope : Scope) =
    let env = Env.live MailboxPlatform (loggingArgs.CreateLogging ()) scope
    let mutable args : CoreAppArgs option = None
    let mutable setupError : exn option = None
    let mutable (* IServicesPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let setupAsync (this : CoreApp) : Task<unit> = task {
        let args' = args |> Option.get
        try
            let! (* IServicesPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args'.Ticker) "Ticker" ""
            ticker <- Some ticker'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsServicesPack args'.PrimaryClipboard) "Clipboard" "Primary"
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args'.LocalHistory) "History" "Local"
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args'.History) "History"
            do! this.SetupAsync' ()
            logInfo env "CoreApp.setupAsync" "Setup_Succeed" (E.encodeJson 4 args')
        with e ->
            setupError <- Some e
            logException env "CoreApp.setupAsync" "Setup_Failed" (E.encodeJson 4 args') e
    }
    member this.Setup (callback : ICoreApp -> unit) (getArgs : unit -> CoreAppArgs) : ICoreApp =
        if args.IsSome then
            failWith "Already_Setup" <| E.encodeJson 4 args.Value
        else
            let args' = getArgs ()
            args <- Some args'
            env.RunTask0 raiseOnFailed (fun _ -> task {
                do! setupAsync this
                match setupError with
                | None -> callback this.AsCoreApp
                | Some e -> raise e
            })
        this.AsCoreApp
    member this.SetupArgs (callback : ICoreApp -> unit) (args' : CoreAppArgs) : ICoreApp =
        fun () -> args'
        |> this.Setup callback
    member this.SetupJson (callback : ICoreApp -> unit) (args' : Json) : ICoreApp =
        fun () ->
            try
                castJson CoreAppArgs.JsonDecoder args'
            with e ->
                logException env "CoreApp.Setup" "Decode_Failed" args e
                raise e
        |> this.Setup callback
    member this.SetupText (callback : ICoreApp -> unit) (args' : string) : ICoreApp =
        parseJson args'
        |> this.SetupJson callback
    member __.SetupError : exn option = setupError
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : CoreAppArgs = args |> Option.get
    interface ICoreApp with
        member this.Args : CoreAppArgs = this.Args
    interface ICorePack with
        member this.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
    interface IServicesPack with
        member this.Args = this.Args.AsServicesPackArgs
        member __.Ticker (* IServicesPack *) : TickerTypes.Agent = ticker |> Option.get
    member this.AsServicesPack = this :> IServicesPack
    member this.AsCorePack = this :> ICorePack
    interface IPack with
        member __.LoggingArgs : LoggingArgs = loggingArgs
        member __.Env : IEnv = env
    interface ILogger with
        member __.Log m = env.Log m
    member this.AsCoreApp = this :> ICoreApp