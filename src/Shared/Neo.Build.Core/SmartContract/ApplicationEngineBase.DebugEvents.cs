// Copyright (C) 2015-2025 The Neo Project.
//
// ApplicationEngineBase.DebugEvents.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Build.Core.Models.SmartContract.Debugger;
using Neo.Build.Core.SmartContract.Debugger;
using Neo.Extensions;
using Neo.SmartContract;
using Neo.VM;
using System;

namespace Neo.Build.Core.SmartContract
{
    public partial class ApplicationEngineBase
    {
        internal void EmitExecuteDebugEvent(VMState result)
        {
            var debugInfo = new NeoDebugEventArgs
            {
                DebugEvent = NeoDebugEvents.Execute,
                Block = PersistingBlock,
                Transaction = CurrentTransaction,
                ScriptHash = GetExecutingScriptHash(),
                DebugInfo = new ExecuteDebugInfoModel()
                {
                    State = result,
                    FeeConsumed = FeeConsumed,
                    GasLeft = GasLeft,
                    ResultStack = ResultStack.ToJson(),
                }
            };

            DebugEvents?.Invoke(this, debugInfo);
        }

        private UInt160 GetExecutingScriptHash()
        {
            ReadOnlyMemory<byte> memoryScript = CurrentContext?.Script ?? ReadOnlyMemory<byte>.Empty;
            return memoryScript.Span.ToScriptHash();
        }
    }
}
