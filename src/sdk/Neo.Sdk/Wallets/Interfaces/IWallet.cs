// Copyright (C) 2015-2024 The Neo Project.
//
// IWallet.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Neo.Sdk.Wallets.Interfaces
{
    public interface IWallet
    {
        string Name { get; }
        Version Version { get; }
        ISCryptParameters SCrypt { get; }
        ICollection<IWalletAccount> Accounts { get; }

        JObject ToJson();
    }
}
