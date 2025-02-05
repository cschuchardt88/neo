// Copyright (C) 2015-2025 The Neo Project.
//
// WalletConfigFileNotFoundException.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.IO;

namespace Neo.Build.Exceptions
{
    /// <inheritdoc />
    internal class WalletConfigFileNotFoundException(
        FileInfo fileInfo) : NeoBuildFileNotFoundException(fileInfo)
    {
        /// <inheritdoc />
        public override int HResult => NeoBuildErrorCodes.Wallet.FileNotFound;
    }
}
