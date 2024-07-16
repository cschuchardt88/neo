// Copyright (C) 2015-2024 The Neo Project.
//
// LinkedListExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.IO;
using Neo.Persistence;
using System.Collections.Generic;

namespace Neo.Extensions
{
    public static class LinkedListExtensions
    {
        public static IEnumerable<byte[]> Seek(this LinkedList<byte[]> list, byte[] keyOrPrefix, SeekDirection direction)
        {
            var first = list.Find(keyOrPrefix);
            first ??= direction == SeekDirection.Forward ?
                    list.First :
                    list.Last;

            var keyComparer = direction == SeekDirection.Forward ? ByteArrayComparer.Default : ByteArrayComparer.Reverse;

            for (; first != null; first = direction == SeekDirection.Forward ? first.Next : first.Previous)
            {
                if (keyComparer.Compare(first.Value, keyOrPrefix) >= 0)
                    yield return first.Value;
            }
        }
    }
}
