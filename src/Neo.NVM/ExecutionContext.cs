// Copyright (C) 2015-2025 The Neo Project.
//
// ExecutionContext.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.NVM.Interfaces;
using System;

namespace Neo.NVM
{
    public class ExecutionContext : IExecutionContext
    {
        public Instruction CurrentInstruction => throw new NotImplementedException();

        public ReadOnlyMemory<byte> Script => _script;

        private readonly Instruction _currentInstruction;
        private readonly ReadOnlyMemory<byte> _script;

        public ExecutionContext(ReadOnlyMemory<byte> script)
        {
            _script = script;
            _currentInstruction = new(script);
        }
    }
}
