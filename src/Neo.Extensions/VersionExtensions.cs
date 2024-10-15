// Copyright (C) 2015-2024 The Neo Project.
//
// VersionExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;

namespace Neo.Extensions
{
    public static class VersionExtensions
    {
        public static int ToNumber(this Version version)
        {
            return version.Major * 1000 + version.Minor * 100 + version.Build * 10 + version.Revision;
        }
    }
}
