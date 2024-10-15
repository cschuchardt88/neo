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
using Neo.Plugins.AutoBackup.Models;
using System;
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

        public void WriteManifest(ManifestModel model)
        {
            var entry = _archive.CreateEntry(ManifestModel.FileName, CompressionLevel.Fastest);
            using var stream = entry.Open();
            var data = model.ToArray();
            stream.Write(data, 0, data.Length);
        }

        public void WriteFile(string filename, ISerializable model)
        {
            var entry = _archive.CreateEntry(filename, CompressionLevel.SmallestSize);
            using var stream = entry.Open();
            var data = model.ToArray();
            stream.Write(data, 0, data.Length);
        }
    }
}
