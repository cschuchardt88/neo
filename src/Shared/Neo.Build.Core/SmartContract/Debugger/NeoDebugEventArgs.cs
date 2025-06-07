// Copyright (C) 2015-2025 The Neo Project.
//
// NeoDebugEventArgs.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Network.P2P.Payloads;

namespace Neo.Build.Core.SmartContract.Debugger
{
    public class NeoDebugEventArgs
    {
        public NeoDebugEvents DebugEvent { init; }

        /// <summary>
        /// Gets the current persisting block, if any.
        /// </summary>
        public Block? Block { init; }

        /// <summary>
        /// Gets the current transaction, if any.
        /// </summary>
        public Transaction? Transaction { init; }

        /// <summary>
        /// <see cref="UInt160"/> of the script container being executed.
        /// </summary>
        public UInt160? ScriptHash { init; }

        /// <summary>
        /// Gets debug information with the current event.
        /// </summary>
        public object? DebugInfo { init; }
    }
}
