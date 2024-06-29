// Copyright (C) 2015-2024 The Neo Project.
//
// UnitTest1.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Network.P2P.Payloads;
using Neo.SmartContract.Native;
using Neo.Test.Framework.SmartContract;
using Neo.VM;

namespace Neo.Test.Framework.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using var sb = new ScriptBuilder()
                .EmitDynamicCall(NativeContract.NEO.Hash, "transfer", UInt160.Zero, UInt160.Zero, 324, null);

            using var testEngine = new TestApplicationEngine(TestBlockchain.GetTestSnapshot(), TestProtocolSettings.Default, new()
            {
                Version = 0,
                Nonce = 0xffffffffu,
                SystemFee = 0,
                NetworkFee = 0,
                ValidUntilBlock = NativeContract.Ledger.CurrentIndex(TestBlockchain.GetTestSnapshot()) + 10000,
                Script = ReadOnlyMemory<byte>.Empty,
                Attributes = [],
                Signers = [
                    new Signer
                    {
                        Account = UInt160.Zero,
                        Scopes = WitnessScope.CalledByEntry,
                        AllowedContracts = [],
                        AllowedGroups = [],
                        Rules = [],
                    },
                ],
                Witnesses = [],
            });

            testEngine.LoadScript(new Script(sb.ToArray()));

            var state = testEngine.Execute();
        }
    }
}
