// Copyright (C) 2015-2024 The Neo Project.
//
// PipeUnmanagedPayload.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Hosting.App.Buffers;
using Neo.Hosting.App.NamedPipes.Protocol.Messages;
using System.Runtime.CompilerServices;

namespace Neo.Hosting.App.NamedPipes.Protocol.Payloads
{
    internal class PipeUnmanagedPayload<T> : IPipeMessage
        where T : unmanaged
    {
        public T Value { get; set; }

        public int Size =>
            Unsafe.SizeOf<T>();

        public void FromArray(byte[] buffer)
        {
            var wrapper = new Struffer(buffer);

            Value = wrapper.Read<T>();
        }

        public byte[] ToArray()
        {
            var wrapper = new Struffer(Size);

            wrapper.Write(Value);

            return [.. wrapper];
        }
    }
}
