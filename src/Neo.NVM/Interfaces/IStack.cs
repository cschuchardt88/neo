// Copyright (C) 2015-2025 The Neo Project.
//
// IStack.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.NVM.Interfaces
{
    internal interface IStack
    {
        /// <summary>
        /// Removes and returns the <see cref="StackItem"/> at the top of the current stack.
        /// </summary>
        /// <returns>The <see cref="StackItem"/> removed from the top of the stack.</returns>
        StackItem Pop();

        /// <summary>
        /// Pushes a <see cref="StackItem"/> onto the top of the current stack.
        /// </summary>
        /// <param name="item">The <see cref="StackItem"/> to be pushed.</param>
        void Push(StackItem item);

        /// <summary>
        /// Returns a <see cref="StackItem"/> at the specified index from the top of the current stack without removing it.
        /// </summary>
        /// <param name="index">The index of the <see cref="StackItem"/> from the top of the stack.</param>
        /// <returns>The item at the specified index.</returns>
        StackItem Peek(int index = 0);

        int Length { get; }
    }
}
