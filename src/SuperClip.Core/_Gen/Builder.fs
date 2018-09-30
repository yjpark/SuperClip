module SuperClip.Core.Builder

open Dap.Context.Helper
open Dap.Context.Builder
open Dap.Prelude
open Dap.Context
open Dap.Platform

(*
 * Generated: <ValueBuilder>
 *)
type PrimaryClipboardArgsBuilder () =
    inherit ObjBuilder<PrimaryClipboardArgs> ()
    override __.Zero () = PrimaryClipboardArgs.Default ()
    [<CustomOperation("check_interval")>]
    member __.CheckInterval (target : PrimaryClipboardArgs, (* PrimaryClipboardArgs *) checkInterval : Duration) =
        target.WithCheckInterval checkInterval
    [<CustomOperation("timeout_duration")>]
    member __.TimeoutDuration (target : PrimaryClipboardArgs, (* PrimaryClipboardArgs *) timeoutDuration : Duration) =
        target.WithTimeoutDuration timeoutDuration

let primary_clipboard_args = PrimaryClipboardArgsBuilder ()

(*
 * Generated: <ValueBuilder>
 *)
type HistoryArgsBuilder () =
    inherit ObjBuilder<HistoryArgs> ()
    override __.Zero () = HistoryArgs.Default ()
    [<CustomOperation("max_size")>]
    member __.MaxSize (target : HistoryArgs, (* HistoryArgs *) maxSize : int) =
        target.WithMaxSize maxSize
    [<CustomOperation("recent_size")>]
    member __.RecentSize (target : HistoryArgs, (* HistoryArgs *) recentSize : int) =
        target.WithRecentSize recentSize

let history_args = HistoryArgsBuilder ()