// Copyright (C) 2015-2024 The Neo Project.
//
// UT_BlockGenerator.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Network.P2P.Payloads;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo.Plugins.AutoBackup.Tests.Helpers
{
    internal class UT_RandomGenerator
    {
        public static byte[] RandomBytes(int length)
        {
            byte[] buffer = new byte[length];
            new Random().NextBytes(buffer);
            return buffer;
        }

        public static ulong RandomUlong()
        {
            byte[] buffer = new byte[sizeof(ulong)];
            new Random().NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static Block RandomBlock(uint index = 0) =>
            new()
            {
                Header = new()
                {
                    Version = 0,
                    PrevHash = new(RandomBytes(UInt256.Length)),
                    MerkleRoot = UInt256.Zero,
                    Timestamp = RandomUlong(),
                    Index = index,
                    NextConsensus = new(RandomBytes(UInt160.Length)),
                    Witness = new()
                    {
                        InvocationScript = Array.Empty<byte>(),
                        VerificationScript = Array.Empty<byte>(),
                    },
                },
                Transactions = [],
            };
    }
}
