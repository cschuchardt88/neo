// Copyright (C) 2015-2025 The Neo Project.
//
// ApplicationEngineBase.SystemIterator.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.SmartContract.Iterators;
using StackItem = Neo.VM.Types.StackItem;

namespace Neo.Build.Core.SmartContract
{
    public partial class ApplicationEngineBase
    {
        protected virtual bool SystemIteratorNext(IIterator iterator)
        {
            return IteratorNext(iterator);
        }

        protected virtual StackItem SystemIteratorValue(IIterator iterator)
        {
            return IteratorValue(iterator);
        }
    }
}
