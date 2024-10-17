// Copyright (C) 2015-2024 The Neo Project.
//
// BackupSettings.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace Neo.Plugins.AutoBackup
{
    internal class BackupSettings : PluginSettings
    {
        public string? Path { get; set; } = "Backups_{0}";
        public bool Auto { get; set; } = false;
        public bool VerifyIntegrity { get; set; } = true;
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;
        public bool SingleFile { get; set; } = false;
        public uint PerBlock { get; set; } = 1000;

        public BackupSettings(IConfigurationSection section) : base(section)
        {
            Path = section.GetValue(nameof(Path), "Backups_{0}");
            Auto = section.GetValue(nameof(Auto), false);
            VerifyIntegrity = section.GetValue(nameof(VerifyIntegrity), true);
            CompressionLevel = section.GetValue(nameof(CompressionLevel), CompressionLevel.Fastest);
            SingleFile = section.GetValue(nameof(SingleFile), false);
            PerBlock = section.GetValue(nameof(PerBlock), 1000u);
        }
    }
}
