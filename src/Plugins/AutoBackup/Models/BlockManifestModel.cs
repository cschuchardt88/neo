// Copyright (C) 2015-2024 The Neo Project.
//
// BlockManifestModel.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Cryptography;
using Neo.Extensions;
using Neo.IO;
using Neo.Network.P2P.Payloads;
using System;
using System.IO;

namespace Neo.Plugins.AutoBackup.Models
{
    internal class BlockManifestModel : ISerializable
    {
        private Block? _block;

        public uint Network { get; set; } = ProtocolSettings.Default.Network;
        public int Version { get; set; } = new Version(1, 0).ToNumber();
        public uint Checksum { get; private set; }
        public Block? Block
        {
            get => _block;
            set
            {
                Checksum = Crc32.Compute(value.ToArray());
                _block = value;
            }
        }

        public int Size =>
            sizeof(uint) +                        // Network
            sizeof(int) +                         // Version
            sizeof(uint) +                        // Crc32 Checksum
            (_block?.Size ?? 0);                  // Block

        public void Deserialize(ref MemoryReader reader)
        {
            Network = reader.ReadUInt32();
            Version = reader.ReadInt32();
            Checksum = reader.ReadUInt32();
            _block = reader.ReadSerializable<Block>();

            if (_block is null)
                throw new FormatException($"Invalid block: {_block}");

            if (Checksum != Crc32.Compute(_block.ToArray()))
                throw new FormatException($"Invalid block checksum: {Checksum}");
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Network);
            writer.Write(Version);
            writer.Write(Checksum);
            writer.Write(_block);
        }
    }
}
