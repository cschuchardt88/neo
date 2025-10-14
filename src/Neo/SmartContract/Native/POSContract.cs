// Copyright (C) 2015-2025 The Neo Project.
//
// POSContract.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.SmartContract.Native
{
    public sealed class POSContract : NativeContract
    {

        private const byte Prefix_Log = 17;

        internal POSContract() : base() { }

        internal override ContractTask InitializeAsync(ApplicationEngine engine, Hardfork? hardfork)
        {
            if (hardfork == ActiveIn)
            {

            }

            return ContractTask.CompletedTask;
        }
    }
}
