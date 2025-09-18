// Copyright (C) 2015-2025 The Neo Project.
//
// JumpTable.cs file belongs to the neo project and is free
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
    public class JumpTable : IJumpTable
    {
        public static readonly JumpTable Default = new();

        private readonly OpcodeMethod[] _opcodeMethodTable = new OpcodeMethod[byte.MaxValue];

        public OpcodeMethod this[OpCode opCode]
        {
            get => _opcodeMethodTable[(byte)opCode];
            internal set => _opcodeMethodTable[(byte)opCode] = value;
        }

        public JumpTable()
        {
            foreach (var method in GetType().GetMethods())
            {
                if (Enum.TryParse<OpCode>(method.Name, true, out var opcodeEnum))
                {
                    var opcode = (byte)opcodeEnum;

                    if (_opcodeMethodTable[opcode] is not null)
                        throw new InvalidOperationException($"Opcode {opcodeEnum} is already defined.");

                    _opcodeMethodTable[opcode] = method.CreateDelegate<OpcodeMethod>(this);
                }
            }

            for (var i = 0; i < _opcodeMethodTable.Length; i++)
            {
                if (_opcodeMethodTable[i] is null)
                    _opcodeMethodTable[i] = InvalidOpcode;
            }
        }

        public virtual void InvalidOpcode(VirtualMachineCore core, Instruction instruction)
        {
            throw new InvalidOperationException($"Opcode {instruction.Code} is undefined.");
        }
    }
}
