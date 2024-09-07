// Copyright (C) 2015-2024 The Neo Project.
//
// ApplicationEngineBuilder.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Network.P2P.Payloads;
using Neo.Persistence;
using Neo.SmartContract;
using Neo.VM;
using System;

namespace Neo.Builders
{
    public sealed class ApplicationEngineBuilder
    {
        private sealed class ApplicationBuilderEngine(
            Transaction tx,
            DataCache snapshot,
            long gas,
            TriggerType triggerType = TriggerType.Application,
            ProtocolSettings protocolSettings = null,
            Block block = null,
            JumpTable jumpTable = null) : ApplicationEngine(
                triggerType,
                tx,
                snapshot,
                block,
                protocolSettings ?? ProtocolSettings.Default,
                gas,
                null,
                jumpTable)
        { }

        private Transaction _container = new();
        private DataCache _snapshot;

        private long _gas = 20_00000000L;

        private TriggerType _triggerType = TriggerType.Application;
        private ProtocolSettings _protocolSettings = ProtocolSettings.Default;

        private Block _block;
        private JumpTable _jumpTable;


        private ApplicationEngineBuilder() { }

        public static ApplicationEngineBuilder CreateEmpty()
        {
            return new ApplicationEngineBuilder();
        }

        public ApplicationEngineBuilder AddScript(Action<ScriptBuilder> config)
        {
            using var sb = new ScriptBuilder();
            config(sb);
            _container.Script = sb.ToArray();
            return this;
        }

        public ApplicationEngine Build()
        {

        }
    }
}
