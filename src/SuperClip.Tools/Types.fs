namespace SuperClip.Tools

open Dap.Platform

exception ParseException of string
exception ExecuteException of string

type App = SuperClip.Server.Types.App