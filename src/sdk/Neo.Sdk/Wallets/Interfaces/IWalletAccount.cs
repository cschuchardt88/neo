// Copyright (C) 2015-2024 The Neo Project.
//
// IWalletAccount.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Wallets;
using Newtonsoft.Json.Linq;

namespace Neo.Sdk.Wallets.Interfaces
{
    public interface IWalletAccount
    {
        UInt160 Address { get; }
        string Label { get; }
        bool IsDefault { get; }
        bool Lock { get; }
        KeyPair Key { get; }
        IWalletContract? Contract { get; }
        JToken Extra { get; }

        JObject ToJson();
    }
}
