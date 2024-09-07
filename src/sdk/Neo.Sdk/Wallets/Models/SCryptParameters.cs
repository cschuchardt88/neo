// Copyright (C) 2015-2024 The Neo Project.
//
// SCryptParameters.cs file belongs to the neo project and is free
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

namespace Neo.Sdk.Wallets.Models
{
    public class SCryptParameters
        (int n, int r, int p) : ISCryptParameters
    {
        public static readonly SCryptParameters Default = new(16384, 8, 8);

        public int N { get; } = n;

        public int R { get; } = r;

        public int P { get; } = p;

        public static SCryptParameters FromJson(string json) =>
            JsonConvert.DeserializeObject<SCryptParameters>(json) ?? throw new NullReferenceException("json");

        public override string ToString() =>
            JsonConvert.SerializeObject(ToJson());

        public JObject ToJson() =>
            new()
            {
                ["n"] = N,
                ["r"] = R,
                ["p"] = P,
            };
    }
}
