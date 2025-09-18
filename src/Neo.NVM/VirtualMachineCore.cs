// Copyright (C) 2015-2025 The Neo Project.
//
// VirtualMachineCore.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.NVM.Collections;
using Neo.NVM.Interfaces;
using System;

namespace Neo.NVM
{
    public class VirtualMachineCore : IExecution
    {
        public ExecutionState State => throw new NotImplementedException();

        //private readonly ExecutionState _state = ExecutionState.BREAK;
        private readonly MemoryStack _stack = new();

        public VirtualMachineCore()
        {

        }

        #region Public

        public ExecutionState Execute()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal

        #endregion
    }
}
