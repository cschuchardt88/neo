// Copyright (C) 2015-2025 The Neo Project.
//
// IStackItem.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.NVM.Interfaces
{
    public interface IStackItem
    {
        /// <summary>
        /// Holds the actual data value.
        /// </summary>
        object? Value { get; set; }

        /// <summary>
        /// Indicates the kind of value.
        /// </summary>
        StackItemType Type { get; set; }

        /// <summary>
        /// Byte size of the value.
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// Flags whether this item is a reference to memory.
        /// </summary>
        bool IsReference { get; set; }

        /// <summary>
        /// Reference counter for memory management.
        /// </summary>
        int RefCount { get; set; }

        /// <summary>
        /// Custom label for debugging or symbolic tracking
        /// </summary>
        string? Tag { get; set; }

        /// <summary>
        /// Tracks which stack frame this item came from.
        /// </summary>
        int OriginFrame { get; set; }

        /// <summary>
        /// Indicates if the value can be modified.
        /// </summary>
        bool IsMutable { get; set; }

        /// <summary>
        /// For profiling or tracing execution timing.
        /// </summary>
        long Timestamp { get; set; }
    }
}
