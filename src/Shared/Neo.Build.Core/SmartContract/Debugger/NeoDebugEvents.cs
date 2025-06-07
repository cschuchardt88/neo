// Copyright (C) 2015-2025 The Neo Project.
//
// NeoDebugEvents.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.Build.Core.SmartContract.Debugger
{
    public enum NeoDebugEvents : short
    {
        Fault = 100,
        Result = 101,

        Create = 200,
        Load = 201,
        PrePost = 202,
        Post = 203,
        Break = 204,
        Execute = 205,

        Burn = 300,
        Call = 301,
        Notify = 302,
        Log = 303,

        Persist = 400,
        PostPersist = 401,

        StoragePut = 500,
        StorageGet = 501,
        StorageFind = 502,
        StorageDelete = 503,

        IteratorMove = 600,
        IteratorGet = 601,
    }
}
