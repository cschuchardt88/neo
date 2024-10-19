// Copyright (C) 2015-2024 The Neo Project.
//
// ArchiveFile.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.IO;
using Neo.Network.P2P.Payloads;
using Neo.Plugins.AutoBackup.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace Neo.Plugins.AutoBackup
{
    internal class ArchiveFile : IDisposable
    {
        private readonly ZipArchive _archive;

        public ArchiveFile(string fileName, bool createFile = true)
        {
            if (createFile)
                _archive = ZipFile.Open(fileName, ZipArchiveMode.Create);
            else
                _archive = ZipFile.Open(fileName, ZipArchiveMode.Update);
        }

        public void Dispose()
        {
            _archive.Dispose();
        }

        public IReadOnlyCollection<string> GetFileNames()
        {
            var fileNames = new string[_archive.Entries.Count];
            for (var i = 0; i < fileNames.Length; i++)
                fileNames[i] = _archive.Entries[i].Name;
            return fileNames;
        }

        public void WriteBlockEntry(Block block, uint network, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            var entry = _archive.CreateEntry($"{block.Index}", compressionLevel);
            using var stream = entry.Open();

            var blockManifest = new BlockManifestModel() { Block = block, Network = network, };
            var data = blockManifest.ToArray();

            stream.Write(data, 0, data.Length);
        }

        public BlockManifestModel? ReadBlockEntry(uint index)
        {
            var entry = _archive.GetEntry($"{index}");
            if (entry is null) return null;

            var bytes = new byte[entry.Length];
            using var stream = entry.Open();

            stream.Read(bytes, 0, bytes.Length);
            return bytes.AsSerializable<BlockManifestModel>();
        }
    }
}
