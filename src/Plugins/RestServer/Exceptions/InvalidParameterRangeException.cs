// Copyright (C) 2015-2024 The Neo Project.
//
// InvalidParameterRangeException.cs file belongs to the neo project and is free
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
    internal class InvalidParameterRangeException : Exception
    {
        public InvalidParameterRangeException() : base() { }
        public InvalidParameterRangeException(string message) : base(message) { }
    }
}
