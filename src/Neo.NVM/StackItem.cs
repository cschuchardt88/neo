// Copyright (C) 2015-2025 The Neo Project.
//
// StackItem.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.NVM.Interfaces;

namespace Neo.NVM
{
    public class StackItem : IStackItem
    {
        /// <inheritdoc/>
        public object? Value { get; set; }

        /// <inheritdoc/>
        public StackItemType Type { get; set; }

        /// <inheritdoc/>
        public int Size { get; set; }

        /// <inheritdoc/>
        public bool IsReference { get; set; }

        /// <inheritdoc/>
        public int RefCount { get; set; }

        /// <inheritdoc/>
        public string? Tag { get; set; }

        /// <inheritdoc/>
        public int OriginFrame { get; set; }

        /// <inheritdoc/>
        public bool IsMutable { get; set; }

        /// <inheritdoc/>
        public long Timestamp { get; set; }
    }
}
