// Copyright (C) 2015-2025 The Neo Project.
//
// Instruction.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.NVM.Attributes;
using Neo.NVM.Exceptions;
using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Neo.NVM
{
    public class Instruction : IEnumerable<Instruction>
    {
        private const int OpCodeSize = 1;

        public int Position { get; private init; }
        public OpCode Code { get; private init; }
        public ReadOnlyMemory<byte> Operand { get; private init; }
        public int OperandSize { get; private init; }
        public int OperandPrefixSize { get; private init; }

        private static readonly int[] s_operandSizeTable = new int[byte.MaxValue + 1];
        private static readonly int[] s_operandSizePrefixTable = new int[byte.MaxValue + 1];

        private readonly ReadOnlyMemory<byte> _script;

        static Instruction()
        {
            foreach (var field in typeof(OpCode).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<OperandSizeAttribute>();
                if (attr == null) continue;

                var index = (byte)(OpCode)field.GetValue(null)!;
                s_operandSizeTable[index] = attr.Size;
                s_operandSizePrefixTable[index] = attr.SizePrefix;
            }
        }

        public Instruction(ReadOnlyMemory<byte> script, int start = 0)
        {
            if (script is { IsEmpty: true })
                throw new BadScriptException();

            if (Enum.IsDefined(typeof(OpCode), script.Span[start]) == false)
                throw new BadScriptException($"Invalid opcode at Position: {start}.");

            OperandPrefixSize = s_operandSizePrefixTable[script.Span[start]];
            OperandSize = OperandPrefixSize switch
            {
                0 => s_operandSizeTable[script.Span[start]],
                1 => script.Span[start + 1],
                2 => BinaryPrimitives.ReadUInt16LittleEndian(script.Span[(start + 1)..]),
                4 => unchecked((int)BinaryPrimitives.ReadUInt32LittleEndian(script.Span[(start + 1)..])),
                _ => throw new BadScriptException($"Invalid opcode prefix at Position: {start}."),
            };

            OperandSize += OperandPrefixSize;

            if (start + OperandSize + OpCodeSize > script.Length)
                throw new BadScriptException($"Operand size exceeds end of script at Position: {start}.");

            Operand = script.Slice(start + OpCodeSize, OperandSize);

            _script = script;
            Code = (OpCode)Enum.ToObject(typeof(OpCode), script.Span[start]);
            Position = start;
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            Instruction currentInstruction;

            yield return this;

            for (var ip = Position + OperandSize + OpCodeSize;
                ip < _script.Length;
                ip += currentInstruction.OperandSize + OpCodeSize)
                yield return currentInstruction = new Instruction(_script, ip);
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public T AsToken<T>(int index = 0)
            where T : unmanaged
        {
            var size = Unsafe.SizeOf<T>();

            if (size > OperandSize)
                throw new ArgumentOutOfRangeException(nameof(T), $"SizeOf {typeof(T).FullName} is too big for operand. OpCode: {Code}.");

            if (size + index > OperandSize)
                throw new ArgumentOutOfRangeException(nameof(index), $"SizeOf {typeof(T).FullName} is too big for operand. OpCode: {Code}.");

            Span<byte> bytes = [.. Operand[..OperandSize].Span];

            return Unsafe.As<byte, T>(ref bytes[index]);
        }
    }
}
