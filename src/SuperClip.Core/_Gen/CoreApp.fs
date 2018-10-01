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

type CoreAppKinds () =
    static member Ticker (* IServicesPack *) = "Ticker"
    static member PrimaryClipboard (* ICorePack *) = "Clipboard"
    static member LocalHistory (* ICorePack *) = "History"
    static member History (* ICorePack *) = "History"

type CoreAppKeys () =
    static member Ticker (* IServicesPack *) = ""
    static member PrimaryClipboard (* ICorePack *) = "Primary"
    static member LocalHistory (* ICorePack *) = "Local"

type ICoreApp =
    inherit IPack
    inherit ICorePack
    abstract Args : CoreAppArgs with get
    abstract AsCorePack : ICorePack with get

(*
 * Generated: <Record>
 *     IsJson, IsLoose
 *)
and CoreAppArgs = {
    Scope : (* CoreAppArgs *) Scope
    Setup : (* CoreAppArgs *) ICoreApp -> unit
    Ticker : (* IServicesPack *) TickerTypes.Args
    PrimaryClipboard : (* ICorePack *) PrimaryTypes.Args
    LocalHistory : (* ICorePack *) HistoryTypes.Args
    History : (* ICorePack *) HistoryTypes.Args
} with
    static member Create scope setup ticker primaryClipboard localHistory history
            : CoreAppArgs =
        {
            Scope = (* CoreAppArgs *) scope
            Setup = (* CoreAppArgs *) setup
            Ticker = (* IServicesPack *) ticker
            PrimaryClipboard = (* ICorePack *) primaryClipboard
            LocalHistory = (* ICorePack *) localHistory
            History = (* ICorePack *) history
        }
    static member Default () =
        CoreAppArgs.Create
            NoScope (* CoreAppArgs *) (* scope *)
            ignore (* CoreAppArgs *) (* setup *)
            (TickerTypes.Args.Default ()) (* IServicesPack *) (* ticker *)
            (PrimaryTypes.Args.Default ()) (* ICorePack *) (* primaryClipboard *)
            (HistoryTypes.Args.Default ()) (* ICorePack *) (* localHistory *)
            (HistoryTypes.Args.Default ()) (* ICorePack *) (* history *)
    static member SetScope ((* CoreAppArgs *) scope : Scope) (this : CoreAppArgs) =
        {this with Scope = scope}
    static member SetSetup ((* CoreAppArgs *) setup : ICoreApp -> unit) (this : CoreAppArgs) =
        {this with Setup = setup}
    static member SetTicker ((* IServicesPack *) ticker : TickerTypes.Args) (this : CoreAppArgs) =
        {this with Ticker = ticker}
    static member SetPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) (this : CoreAppArgs) =
        {this with PrimaryClipboard = primaryClipboard}
    static member SetLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) (this : CoreAppArgs) =
        {this with LocalHistory = localHistory}
    static member SetHistory ((* ICorePack *) history : HistoryTypes.Args) (this : CoreAppArgs) =
        {this with History = history}
    static member UpdateScope ((* CoreAppArgs *) update : Scope -> Scope) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetScope (update this.Scope)
    static member UpdateTicker ((* IServicesPack *) update : TickerTypes.Args -> TickerTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetTicker (update this.Ticker)
    static member UpdatePrimaryClipboard ((* ICorePack *) update : PrimaryTypes.Args -> PrimaryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetPrimaryClipboard (update this.PrimaryClipboard)
    static member UpdateLocalHistory ((* ICorePack *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetLocalHistory (update this.LocalHistory)
    static member UpdateHistory ((* ICorePack *) update : HistoryTypes.Args -> HistoryTypes.Args) (this : CoreAppArgs) =
        this |> CoreAppArgs.SetHistory (update this.History)
    static member JsonEncoder : JsonEncoder<CoreAppArgs> =
        fun (this : CoreAppArgs) ->
            E.object [
                "scope", Scope.JsonEncoder (* CoreAppArgs *) this.Scope
                "ticker", TickerTypes.Args.JsonEncoder (* IServicesPack *) this.Ticker
                "primary_clipboard", PrimaryTypes.Args.JsonEncoder (* ICorePack *) this.PrimaryClipboard
                "local_history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.LocalHistory
                "history", HistoryTypes.Args.JsonEncoder (* ICorePack *) this.History
            ]
    static member JsonDecoder : JsonDecoder<CoreAppArgs> =
        D.decode CoreAppArgs.Create
        |> D.optional (* CoreAppArgs *) "scope" Scope.JsonDecoder NoScope
        |> D.hardcoded (* CoreAppArgs *) (* Setup *) ignore
        |> D.optional (* IServicesPack *) "ticker" TickerTypes.Args.JsonDecoder (TickerTypes.Args.Default ())
        |> D.optional (* ICorePack *) "primary_clipboard" PrimaryTypes.Args.JsonDecoder (PrimaryTypes.Args.Default ())
        |> D.optional (* ICorePack *) "local_history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
        |> D.optional (* ICorePack *) "history" HistoryTypes.Args.JsonDecoder (HistoryTypes.Args.Default ())
    static member JsonSpec =
        FieldSpec.Create<CoreAppArgs>
            CoreAppArgs.JsonEncoder CoreAppArgs.JsonDecoder
    interface IJson with
        member this.ToJson () = CoreAppArgs.JsonEncoder this
    interface IObj
    member this.WithScope ((* CoreAppArgs *) scope : Scope) =
        this |> CoreAppArgs.SetScope scope
    member this.WithSetup ((* CoreAppArgs *) setup : ICoreApp -> unit) =
        this |> CoreAppArgs.SetSetup setup
    member this.WithTicker ((* IServicesPack *) ticker : TickerTypes.Args) =
        this |> CoreAppArgs.SetTicker ticker
    member this.WithPrimaryClipboard ((* ICorePack *) primaryClipboard : PrimaryTypes.Args) =
        this |> CoreAppArgs.SetPrimaryClipboard primaryClipboard
    member this.WithLocalHistory ((* ICorePack *) localHistory : HistoryTypes.Args) =
        this |> CoreAppArgs.SetLocalHistory localHistory
    member this.WithHistory ((* ICorePack *) history : HistoryTypes.Args) =
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
    [<CustomOperation("scope")>]
    member __.Scope (target : CoreAppArgs, (* CoreAppArgs *) scope : Scope) =
        target.WithScope scope
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

let core_app_args = CoreAppArgsBuilder ()

type CoreApp (logging : ILogging, args : CoreAppArgs) as this =
    let env = Env.live MailboxPlatform logging args.Scope
    let mutable setupError : exn option = None
    let mutable (* IServicesPack *) ticker : TickerTypes.Agent option = None
    let mutable (* ICorePack *) primaryClipboard : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> option = None
    let mutable (* ICorePack *) localHistory : HistoryTypes.Agent option = None
    let setupAsync (_runner : IRunner) : Task<unit> = task {
        try
            let! (* IServicesPack *) ticker' = env |> Env.addServiceAsync (Dap.Platform.Ticker.Logic.spec args.Ticker) CoreAppKinds.Ticker CoreAppKeys.Ticker
            ticker <- Some ticker'
            let! (* ICorePack *) primaryClipboard' = env |> Env.addServiceAsync (SuperClip.Core.Primary.Logic.spec this.AsServicesPack args.PrimaryClipboard) CoreAppKinds.PrimaryClipboard CoreAppKeys.PrimaryClipboard
            primaryClipboard <- Some (primaryClipboard' :> IAgent<PrimaryTypes.Req, PrimaryTypes.Evt>)
            let! (* ICorePack *) localHistory' = env |> Env.addServiceAsync (SuperClip.Core.History.Logic.spec args.LocalHistory) CoreAppKinds.LocalHistory CoreAppKeys.LocalHistory
            localHistory <- Some localHistory'
            do! env |> Env.registerAsync (SuperClip.Core.History.Logic.spec (* ICorePack *) args.History) CoreAppKinds.History
            do! this.SetupAsync' ()
            logInfo env "CoreApp.setupAsync" "Setup_Succeed" (E.encodeJson 4 args)
            args.Setup this.AsCoreApp
        with e ->
            setupError <- Some e
            logException env "CoreApp.setupAsync" "Setup_Failed" (E.encodeJson 4 args) e
            raise e
    }
    do (
        env.RunTask0 raiseOnFailed setupAsync
    )
    new (loggingArgs : LoggingArgs, a : CoreAppArgs) =
        CoreApp (loggingArgs.CreateLogging (), a)
    new (a : CoreAppArgs) =
        CoreApp (getLogging (), a)
    abstract member SetupAsync' : unit -> Task<unit>
    default __.SetupAsync' () = task {
        return ()
    }
    member __.Args : CoreAppArgs = args
    member __.Env : IEnv = env
    member __.SetupError : exn option = setupError
    interface ILogger with
        member __.Log m = env.Log m
    interface IPack with
        member __.Env : IEnv = env
    interface IServicesPack with
        member __.Args = this.Args.AsServicesPackArgs
        member __.Ticker (* IServicesPack *) : TickerTypes.Agent = ticker |> Option.get
    member __.AsServicesPack = this :> IServicesPack
    interface ICorePack with
        member __.Args = this.Args.AsCorePackArgs
        member __.PrimaryClipboard (* ICorePack *) : IAgent<PrimaryTypes.Req, PrimaryTypes.Evt> = primaryClipboard |> Option.get
        member __.LocalHistory (* ICorePack *) : HistoryTypes.Agent = localHistory |> Option.get
        member __.GetHistoryAsync (key : Key) (* ICorePack *) : Task<HistoryTypes.Agent * bool> = task {
            let! (agent, isNew) = env.HandleAsync <| DoGetAgent "History" key
            return (agent :?> HistoryTypes.Agent, isNew)
        }
        member __.AsServicesPack = this.AsServicesPack
    member __.AsCorePack = this :> ICorePack
    interface ICoreApp with
        member __.Args : CoreAppArgs = this.Args
        member __.AsCorePack = this.AsCorePack
    member __.AsCoreApp = this :> ICoreApp