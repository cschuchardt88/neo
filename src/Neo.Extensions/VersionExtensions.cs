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
            var number = 0;
            if (version.Major >= 0)
                number += version.Major * 1000;
            if (version.Minor >= 0)
                number += version.Minor * 100;
            if (version.Build >= 0)
                number += version.Build * 10;
            if (version.Revision >= 0)
                number += version.Revision;
            return number;
        }
    }
}
