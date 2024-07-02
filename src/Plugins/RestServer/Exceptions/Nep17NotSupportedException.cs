// Copyright (C) 2015-2024 The Neo Project.
//
// Nep17NotSupportedException.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;

namespace Neo.Plugins.RestServer.Exceptions
{
    internal class Nep17NotSupportedException : Exception
    {
        public Nep17NotSupportedException() { }
        public Nep17NotSupportedException(UInt160 scriptHash) : base($"Contract '{scriptHash}' does not support NEP-17.") { }
    }
}
