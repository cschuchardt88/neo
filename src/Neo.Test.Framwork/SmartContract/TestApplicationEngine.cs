// Copyright (C) 2015-2024 The Neo Project.
//
// TestApplicationEngine.cs file belongs to the neo project and is free
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
using OneOf;
using System;
using System.Collections.Generic;

namespace Neo.Test.Framework.SmartContract
{
    public class TestApplicationEngine : ApplicationEngine
    {
        private readonly Dictionary<UInt160, OneOf<ContractState, Script>> _executedScripts = new();

        public TestApplicationEngine(DataCache snapshot, ProtocolSettings settings, Transaction transaction)
            : this(snapshot, container: transaction, settings: settings) { }

        public TestApplicationEngine(
            DataCache snapshot,
            TriggerType trigger = TriggerType.Application,
            IVerifiable? container = null,
            Block? persistingBlock = null,
            ProtocolSettings? settings = null,
            long gas = 20_0000000L,
            IDiagnostic? diagnostic = null,
            JumpTable? jumpTable = null) :
            base(trigger, container, snapshot, persistingBlock, settings, gas, diagnostic, jumpTable)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        public override VMState Execute()
        {
            return base.Execute();
        }

        public override void LoadContext(ExecutionContext context)
        {
            base.LoadContext(context);

            var executionContextState = context.GetState<ExecutionContextState>();

            if (executionContextState.ScriptHash != null)
                _executedScripts.TryAdd(
                    executionContextState.ScriptHash,
                    executionContextState.Contract == null ? context.Script : executionContextState.Contract);
        }

        protected override void PreExecuteInstruction(Instruction instruction)
        {
            base.PreExecuteInstruction(instruction);
        }

        protected override void OnSysCall(InteropDescriptor descriptor)
        {
            base.OnSysCall(descriptor);
        }
    }
}
