// Copyright (C) 2015-2024 The Neo Project.
//
// PipeEmptyPayload.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.IO;

namespace Neo.Hosting.App.NamedPipes.Protocol.Messages
{
    internal sealed class PipeEmptyPayload : IPipeMessage
    {
        public int Size => 0;

        public void CopyFrom(Stream stream) { }

        public void FromArray(byte[] buffer) { }

        public void CopyTo(Stream stream) { }

        public void CopyTo(byte[] buffer) { }

        public byte[] ToArray() => [];
    }
}