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

using static System.IO.Path;

namespace Neo.Plugins.AutoBackup
{
    public class AutoBackupPlugin : Plugin
    {
        #region Globals

        private NeoSystem? _neoSystem;

        #endregion

        #region Overrides

        public override string Name => "AutoBackup";
        public override string Description => "Take local snapshots of the blockchain.";

        public override string ConfigFile => Combine(RootPath, "AutoBackup.json");

        protected override UnhandledExceptionPolicy ExceptionPolicy => UnhandledExceptionPolicy.Ignore;

        public override void Dispose()
        {

        }

        protected override void Configure()
        {

        }

        protected override void OnSystemLoaded(NeoSystem system)
        {
            _neoSystem = system;
        }

        #endregion
    }
}
