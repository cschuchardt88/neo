// Copyright (C) 2015-2025 The Neo Project.
//
// RpcUnclaimedGas.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the repository
// or https://opensource.org/license/mit for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Json;

namespace Neo.Network.RPC.Models
{
    public class RpcUnclaimedGas
    {
        public long Unclaimed { get; set; }

        public string Address { get; set; }

        public JObject ToJson()
        {
            JObject json = new();
            json["unclaimed"] = Unclaimed.ToString();
            json["address"] = Address;
            return json;
        }

        public static RpcUnclaimedGas FromJson(JObject json)
        {
            return new RpcUnclaimedGas
            {
                Unclaimed = long.Parse(json["unclaimed"].AsString()),
                Address = json["address"].AsString()
            };
        }
    }
}
