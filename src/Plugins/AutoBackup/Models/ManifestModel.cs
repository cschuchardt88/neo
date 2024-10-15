// Copyright (C) 2015-2024 The Neo Project.
//
// ManifestModel.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Extensions;
using Neo.IO;
using System;
using System.IO;

namespace Neo.Plugins.AutoBackup.Models
{
    internal class ManifestModel : ISerializable
    {
        public const string FileName = "[MANIFEST]";

        public uint Network { get; set; } = ProtocolSettings.Default.Network;
        public int Version { get; set; } = new Version("1.0").ToNumber();
        public uint[] ChecksumTable { get; set; } = [];

        public int Size =>
            sizeof(uint) +                        // Network
            sizeof(int) +                         // Version
            (sizeof(int) * ChecksumTable.Length); // ChecksumTable

        public void Deserialize(ref MemoryReader reader)
        {
            Network = reader.ReadUInt32();
            Version = reader.ReadInt32();

            var len = reader.ReadInt32();
            for (var i = 0; i < len; i++)
                ChecksumTable = [.. ChecksumTable, reader.ReadUInt32()];
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Network);
            writer.Write(Version);
            writer.Write(ChecksumTable.Length);
            foreach (var checksum in ChecksumTable)
                writer.Write(checksum);
        }
    }
}
