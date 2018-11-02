[<RequireQualifiedAccess>]
module SuperClip.Eto.Feature.AppGui

open FSharp.Control.Tasks.V2

open Dap.Prelude
open Dap.Context
open Dap.Platform
open Dap.Gui

open SuperClip.App

type Context (logging : ILogging) =
    inherit BaseAppGui<Context> (logging)
    do (
        let owner = base.AsOwner
        base.DoLogin.SetupHandler (fun () ->
            logWip owner "AppGui:DoLogin" ()
        )
    )
    override this.Self = this
    override __.Spawn l = new Context (l)
    static member AddToAgent (agent : IAgent) =
        new Context (agent.Env.Logging) :> IAppGui
