// Copyright (C) 2015-2024 The Neo Project.
//
// RegexUtility.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Text.RegularExpressions;

namespace Neo.Service
{
    internal static partial class RegexUtility
    {
        [GeneratedRegex(@"\d+")]
        public static partial Regex SearchNumbersOnly();
    }
}