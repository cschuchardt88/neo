// Copyright (C) 2015-2024 The Neo Project.
//
// AutoBackupPlugin.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.ConsoleService;
using Neo.Cryptography;
using Neo.IO;
using Neo.Plugins.AutoBackup.Models;
using Neo.SmartContract.Native;
using System.IO;
using static System.IO.Path;

namespace Neo.Plugins.AutoBackup
{
    public class AutoBackupPlugin : Plugin
    {
        #region Globals

        private NeoSystem? _neoSystem;
        private BackupSettings? _settings;
        private ManifestModel? _manifest;

        #endregion

        #region Overrides

        public override string Name => "AutoBackup";
        public override string Description => "Take local snapshots of the blockchain.";

        public override string ConfigFile => Combine(RootPath, "AutoBackup.json");

        protected override UnhandledExceptionPolicy ExceptionPolicy => _settings?.ExceptionPolicy ?? UnhandledExceptionPolicy.Ignore;

        public override void Dispose()
        {

        }

        protected override void Configure()
        {
            _settings = new BackupSettings(GetConfiguration());
        }

        protected override void OnSystemLoaded(NeoSystem system)
        {
            _neoSystem = system;
            _manifest = new()
            {
                Network = system.Settings.Network
            };
        }

        #endregion

        #region Commands

        [ConsoleCommand("backup", Category = "AutoBackup")]
        public void onBackup(uint start, uint end)
        {
            if (start > end)
            {
                ConsoleHelper.Warning("Start block must be less than or equal to end block.");
                return;
            }

            var height = NativeContract.Ledger.CurrentIndex(_neoSystem?.StoreView);

            if (start > height)
            {
                ConsoleHelper.Warning("Start block must be less than or equal to the current block height.");
                return;
            }

            if (end > height)
            {
                ConsoleHelper.Warning("End block must be less than or equal to the current block height.");
                return;
            }

            var dir = string.Format(_settings!.Path!, _neoSystem!.Settings.Network);
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            var filename = string.Format("{0}\\arc.{1}.afc", dir, start);
            if (File.Exists(filename))
                File.Delete(filename);

            using var archiveFile = new ArchiveFile(filename, true);

            for (var idx = 0u; idx <= end; idx++)
            {
                var block = NativeContract.Ledger.GetBlock(_neoSystem!.StoreView, idx);
                var checksum = Crc32.Compute(block.ToArray());
                _manifest!.ChecksumTable = [.. _manifest!.ChecksumTable, checksum];
                archiveFile.WriteFile($"{idx}", block);
            }

            archiveFile.WriteManifest(_manifest!);

            ConsoleHelper.Info($"Backup created at {filename}.");
        }

        #endregion
    }
}
