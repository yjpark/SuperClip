module SuperClip.Core.CoreApp

open Dap.Context.Helper
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
    Ticker : (* CoreAppArgs *) TickerTypes.Args
    PrimaryClipboard : (* CoreAppArgs *) PrimaryTypes.Args
    LocalHistory : (* CoreAppArgs *) HistoryTypes.Args
    History : (* CoreAppArgs *) HistoryTypes.Args
} with
    static member Create ticker primaryClipboard localHistory history
            : CoreAppArgs =
        {
            Ticker = (* CoreAppArgs *) ticker
            PrimaryClipboard = (* CoreAppArgs *) primaryClipboard
            LocalHistory = (* CoreAppArgs *) localHistory
            History = (* CoreAppArgs *) history
        }
    static member Default () =
        CoreAppArgs.Create
            (TickerTypes.Args.Default ()) (* CoreAppArgs *) (* ticker *)
            (PrimaryTypes.Args.Default ()) (* CoreAppArgs *) (* primaryClipboard *)
            (HistoryTypes.Args.Default ()) (* CoreAppArgs *) (* localHistory *)
            (HistoryTypes.Args.Default ()) (* CoreAppArgs *) (* history *)
    static member SetTicker ((* CoreAppArgs *) ticker : TickerTypes.Args) (this : CoreAppArgs) =
        {this with Ticker = ticker}
    static member SetPrimaryClipboard ((* CoreAppArgs *) primaryClipboard : PrimaryTypes.Args) (this : CoreAppArgs) =
        {this with PrimaryClipboard = primaryClipboard}
    static member SetLocalHistory ((* CoreAppArgs *) localHistory : HistoryTypes.Args) (this : CoreAppArgs) =
        {this with LocalHistory = localHistory}
    static member SetHistory ((* CoreAppArgs *) history : HistoryTypes.Args) (this : CoreAppArgs) =
        {this with History = history}
    static member UpdateTicker ((* CoreAppArgs *) update : TickerTypes.Args -> TickerTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetTicker (update this.Ticker)
    static member UpdatePrimaryClipboard ((* CoreAppArgs *) update : PrimaryTypes.Args -> PrimaryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetPrimaryClipboard (update this.PrimaryClipboard)
    static member UpdateLocalHistory ((* CoreAppArgs *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetLocalHistory (update this.LocalHistory)
    static member UpdateHistory ((* CoreAppArgs *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetHistory (update this.History)
    static member JsonEncoder : JsonEncoder<CoreAppArgs> =
        fun (this : CoreAppArgs) ->
            E.object [
                "ticker", TickerTypes.Args.JsonEncoder (* CoreAppArgs *) this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder (* CoreAppArgs *) this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder (* CoreAppArgs *) this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder (* CoreAppArgs *) this.History
            ]
    static member JsonDecoder : JsonDecoder<CoreAppArgs> =
        D.decode CoreAppArgs.Create
        |> D.optional (* CoreAppArgs *) "ticker" TickerTypes.Args.JsonDecoder (TickerTypes.Args.Default ())
        |> D.optional (* CoreAppArgs *) "primary_clipboard" PrimaryTypes.Args.JsonDecoder (PrimaryTypes.Args.Default ())
        |> D.optional (* CoreAppArgs *) "local_history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
        |> D.optional (* CoreAppArgs *) "history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
    static member JsonSpec =
        FieldSpec.Create<CoreAppArgs>
            CoreAppArgs.JsonEncoder CoreAppArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = CoreAppArgs.JsonEncoder this
    interface IObj
    member this.WithTicker ((* CoreAppArgs *) ticker : TickerTypes.Args) =
        this |> CoreAppArgs.SetTicker ticker
    member this.WithPrimaryClipboard ((* CoreAppArgs *) primaryClipboard : PrimaryTypes.Args) =
        this |> CoreAppArgs.SetPrimaryClipboard primaryClipboard
    member this.WithLocalHistory ((* CoreAppArgs *) localHistory : HistoryTypes.Args) =
        this |> CoreAppArgs.SetLocalHistory localHistory
    member this.WithHistory ((* CoreAppArgs *) history : HistoryTypes.Args) =
        this |> CoreAppArgs.SetHistory history
    interface IServicesPackArgs with
        member this.Ticker (* IServicesPack *) : TickerTypes.Args = this.Ticker
    member this.AsServicesPackArgs = this :> IServicesPackArgs
    interface ICorePackArgs with
        member this.PrimaryClipboard (* ICorePack *) : PrimaryTypes.Args = this.PrimaryClipboard
        member this.LocalHistory (* ICorePack *) : HistoryTypes.Args = this.LocalHistory
        member this.History (* ICorePack *) : HistoryTypes.Args = this.History
        member this.AsServicesPackArgs = this.AsServicesPackArgs
    member this.AsCorePackArgs = this :> ICorePackArgs

(*
 * Generated: <ValueBuilder>
 *)
type CoreAppArgsBuilder () =
    inherit ObjBuilder<CoreAppArgs> ()
    override __.Zero () = CoreAppArgs.Default ()
    [<CustomOperation("ticker")>]
    member __.Ticker (target : CoreAppArgs, (* CoreAppArgs *) ticker : TickerTypes.Args) =
        target.WithTicker ticker
    [<CustomOperation("primary_clipboard")>]
    member __.PrimaryClipboard (target : CoreAppArgs, (* CoreAppArgs *) primaryClipboard : PrimaryTypes.Args) =
        target.WithPrimaryClipboard primaryClipboard
    [<CustomOperation("local_history")>]
    member __.LocalHistory (target : CoreAppArgs, (* CoreAppArgs *) localHistory : HistoryTypes.Args) =
        target.WithLocalHistory localHistory
    [<CustomOperation("history")>]
    member __.History (target : CoreAppArgs, (* CoreAppArgs *) history : HistoryTypes.Args) =
        target.WithHistory history

let core_app_args = CoreAppArgsBuilder ()

type ICoreApp =
    inherit IPack
    inherit ICorePack
    abstract Args : CoreAppArgs with get
    abstract AsCorePack : ICorePack with get

type CoreAppKinds () =
    static member Ticker (* IServicesPack *) = "Ticker"
    static member PrimaryClipboard (* ICorePack *) = "Clipboard"
    static member LocalHistory (* ICorePack *) = "History"
    static member History (* ICorePack *) = "History"

type CoreAppKeys () =
    static member Ticker (* IServicesPack *) = ""
    static member PrimaryClipboard (* ICorePack *) = "Primary"
    static member LocalHistory (* ICorePack *) = "Local"

type CoreApp (logging : ILogging, scope : Scope) =
    let env = Env.live MailboxPlatform logging scope
    let mutable args : CoreAppArgs option = None
    let mutable setupError : exn option = None
    let mutable (* IServicesPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let setupAsync (this : CoreApp) : Task<unit> = task {
        let args' = args |> Option.get
        try
            let! (* IServicesPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args'.Ticker) CoreAppKinds.Ticker CoreAppKeys.Ticker
            ticker <- Some ticker'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsServicesPack args'.PrimaryClipboard) CoreAppKinds.PrimaryClipboard CoreAppKeys.PrimaryClipboard
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args'.LocalHistory) CoreAppKinds.LocalHistory CoreAppKeys.LocalHistory
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args'.History) CoreAppKinds.History
            do! this.SetupAsync' ()
            logInfo env "CoreApp.setupAsync" "Setup_Succeed" (E.encodeJson 4 args')
        with e ->
            setupError <- Some e
            logException env "CoreApp.setupAsync" "Setup_Failed" (E.encodeJson 4 args') e
    }
    new (loggingArgs : LoggingArgs, scope : Scope) =
        CoreApp (loggingArgs.CreateLogging (), scope)
    new (scope : Scope) =
        CoreApp (getLogging (), scope)
    member this.SetupAsync (getArgs : unit -> CoreAppArgs) : Task<unit> = task {
        if args.IsSome then
            failWith "Already_Setup" <| E.encodeJson 4 args.Value
        else
            let args' = getArgs ()
            args <- Some args'
            do! setupAsync this
            match setupError with
            | None -> ()
            | Some e -> raise e
        return ()
        }
    member this.SetupAsync (args' : CoreAppArgs) : Task<unit> =
        fun () -> args'
        |> this.SetupAsync
    member this.SetupAsync (args' : Json) : Task<unit> =
        fun () ->
            try
                castJson CoreAppArgs.JsonDecoder args'
            with e ->
                logException env "CoreApp.SetupAsync" "Decode_Failed" args e
                raise e
        |> this.SetupAsync
    member this.SetupAsync (args' : string) : Task<unit> =
        let json : Json = parseJson args'
        this.SetupAsync json
    member __.SetupError : exn option = setupError
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : CoreAppArgs = args |> Option.get
    interface ILogger with
        member __.Log m = env.Log m
    interface IPack with
        member __.Env : IEnv = env
    interface IServicesPack with
        member this.Args = this.Args.AsServicesPackArgs
        member __.Ticker (* IServicesPack *) : TickerTypes.Agent = ticker |> Option.get
    member this.AsServicesPack = this :> IServicesPack
    interface ICorePack with
        member this.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
        member this.AsServicesPack = this.AsServicesPack
    member this.AsCorePack = this :> ICorePack
    interface ICoreApp with
        member this.Args : CoreAppArgs = this.Args
        member this.AsCorePack = this.AsCorePack
    member this.AsCoreApp = this :> ICoreApp