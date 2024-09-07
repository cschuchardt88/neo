// Copyright (C) 2015-2024 The Neo Project.
//
// Nep6Wallet.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Sdk.Wallets.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Sdk.Wallets.Models
{
    public class Nep6Wallet : IWallet
    {
        public string Name { get; set; } = default!;

        public Version Version { get; } = new("1.0");

        public ISCryptParameters SCrypt { get; } = SCryptParameters.Default;

        public ICollection<IWalletAccount> Accounts { get; set; } = [];

        public static Nep6Wallet FromJson(string json) =>
            JsonConvert.DeserializeObject<Nep6Wallet>(json) ?? throw new NullReferenceException("json");

        public override string ToString() =>
            JsonConvert.SerializeObject(ToJson());

        public JObject ToJson() =>
            new()
            {
                ["name"] = Name,
                ["version"] = Version.ToString(2),
                ["scrypt"] = SCrypt.ToJson(),
                ["accounts"] = JArray.FromObject(Accounts.Select(s => s.ToJson()).ToArray()),
                ["extra"] = null,
            };
    }
}
