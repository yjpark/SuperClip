module SuperClip.Core.Channels.Types

open Dap.Prelude
open Dap.Platform

type Args = NoArgs

and Model<'extra, 'evt> = {
    Channels : Map<ChannelKey, Channel * 'extra>
}