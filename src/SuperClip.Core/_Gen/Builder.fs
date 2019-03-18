module SuperClip.Core.Builder

open Dap.Context.Builder
open Dap.Prelude
open Dap.Context
open Dap.Platform

(*
 * Generated: <ValueBuilder>
 *)
type PrimaryClipboardArgsBuilder () =
    inherit ObjBuilder<PrimaryClipboardArgs> ()
    override __.Zero () = PrimaryClipboardArgs.Create ()
    [<CustomOperation("check_interval")>]
    member __.CheckInterval (target : PrimaryClipboardArgs, (* PrimaryClipboardArgs *) checkInterval : Duration) =
        target.WithCheckInterval checkInterval
    [<CustomOperation("timeout_duration")>]
    member __.TimeoutDuration (target : PrimaryClipboardArgs, (* PrimaryClipboardArgs *) timeoutDuration : Duration) =
        target.WithTimeoutDuration timeoutDuration

let primary_clipboard_args = new PrimaryClipboardArgsBuilder ()

(*
 * Generated: <ValueBuilder>
 *)
type HistoryArgsBuilder () =
    inherit ObjBuilder<HistoryArgs> ()
    override __.Zero () = HistoryArgs.Create ()
    [<CustomOperation("pinned_size")>]
    member __.PinnedSize (target : HistoryArgs, (* HistoryArgs *) pinnedSize : int) =
        target.WithPinnedSize pinnedSize
    [<CustomOperation("recent_size")>]
    member __.RecentSize (target : HistoryArgs, (* HistoryArgs *) recentSize : int) =
        target.WithRecentSize recentSize

let history_args = new HistoryArgsBuilder ()