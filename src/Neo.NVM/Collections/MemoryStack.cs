// Copyright (C) 2015-2025 The Neo Project.
//
// MemoryStack.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.NVM.Interfaces;
using System;

namespace Neo.NVM.Collections
{
    internal class MemoryStack : IStack
    {
        public int Length => _stackItems.Length;

        private Memory<StackItem> _stackItems;

        public StackItem Peek(int index) =>
            _stackItems.Span[_stackItems.Length - index - 1];

        public void Push(StackItem item) =>
            _stackItems = new Memory<StackItem>([.. _stackItems.ToArray(), item]);

        public StackItem Pop()
        {
            var index = _stackItems.Length - 1;
            var result = _stackItems.Span[index];
            _stackItems = _stackItems[..index];

            return result;
        }
    }
}
