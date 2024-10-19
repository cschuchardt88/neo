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
using Neo.SmartContract.Native;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.IO.Path;

namespace Neo.Plugins.AutoBackup
{
    public class AutoBackupPlugin : Plugin
    {
        #region Globals

        private NeoSystem? _neoSystem;
        private BackupSettings? _settings;
        private Task? _autoBackupTask;
        private readonly CancellationTokenSource _ctsAutoBackup = new();

        #endregion

        #region Overrides

        public override string Name => "AutoBackup";
        public override string Description => "Take local snapshots of the blockchain.";

        public override string ConfigFile => Combine(RootPath, "AutoBackup.json");

        protected override UnhandledExceptionPolicy ExceptionPolicy => _settings?.ExceptionPolicy ?? UnhandledExceptionPolicy.Ignore;

        public override void Dispose()
        {
            _ctsAutoBackup.Cancel();
            _autoBackupTask?.Wait();
        }

        protected override void Configure()
        {
            _settings = new BackupSettings(GetConfiguration());
        }

        protected override void OnSystemLoaded(NeoSystem system)
        {
            _neoSystem = system;
            _autoBackupTask = Task.Run(AutoBackup, _ctsAutoBackup.Token);
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

            var filename = string.Format(@"{0}\chain.{1}.{2}.arc", dir, start, end);
            if (File.Exists(filename))
            {
                var answer = ConsoleHelper.ReadUserInput("Backup file already exists. Overwrite? (y/n)");
                if (answer.ToLowerInvariant() is "yes" or "y")
                    File.Delete(filename);
            }

            using var archiveFile = new ArchiveFile(filename, true);

            Console.CursorVisible = false;

            var (csrLeft, csrTop) = Console.GetCursorPosition();

            for (var idx = 0u; idx <= end; idx++)
            {
                var block = NativeContract.Ledger.GetBlock(_neoSystem!.StoreView, idx);
                archiveFile.WriteBlockEntry(block, _neoSystem!.Settings.Network, _settings!.CompressionLevel);

                Console.SetCursorPosition(csrLeft, csrTop);
                ConsoleHelper.Info("Block ", $"{idx}", $" of ", $"{end}", " archived.");
            }

            ConsoleHelper.Info("Backup created at ", $"{filename}", ".");
            Console.CursorVisible = true;
        }

        #endregion

        #region Threads

        public async Task AutoBackup()
        {
            if (_settings!.Auto == false)
                return;

            try
            {
                var dir = string.Format(_settings!.Path!, _neoSystem!.Settings.Network);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                var fileName = string.Format(@"{0}\chain.arc", dir);
                var fileExists = File.Exists(fileName);
                using var archiveFile = new ArchiveFile(fileName, !fileExists);

                var height = NativeContract.Ledger.CurrentIndex(_neoSystem!.StoreView);
                var lastBlockIndex = fileExists ? archiveFile.GetFileNames().Max(uint.Parse) : 0;
                while (_ctsAutoBackup.IsCancellationRequested == false)
                {
                    for (var idx = lastBlockIndex; idx <= height; idx++)
                    {
                        if (_ctsAutoBackup.IsCancellationRequested)
                            break;

                        var block = NativeContract.Ledger.GetBlock(_neoSystem!.StoreView, idx);
                        archiveFile.WriteBlockEntry(block, _neoSystem!.Settings.Network, _settings!.CompressionLevel);
                    }

                    await Task.Delay(_neoSystem!.Settings.TimePerBlock, _ctsAutoBackup.Token);
                }
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                ConsoleHelper.Error(ex.Message);
            }
        }

        #endregion
    }
}
