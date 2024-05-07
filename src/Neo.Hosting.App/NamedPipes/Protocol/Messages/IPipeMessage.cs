// Copyright (C) 2015-2024 The Neo Project.
//
// IPipeMessage.cs file belongs to the neo project and is free
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
    internal interface IPipeMessage
    {
        int Size { get; }
        void CopyTo(Stream stream);
        void CopyFrom(Stream stream);
        void CopyTo(byte[] buffer, int start = 0);
        void CopyFrom(byte[] buffer, int start = 0);
        byte[] ToArray();
    }
}
