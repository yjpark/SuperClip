namespace SuperClip.Tools

open Dap.Platform

exception ParseException of string
exception ExecuteException of string

type ClientApp = SuperClip.App.App.App
type ServerApp = SuperClip.Server.App.App

type IClientApp = SuperClip.App.IApp.IApp
type IServerApp = SuperClip.Server.App.IApp
