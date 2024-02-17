// Copyright (C) 2015-2024 The Neo Project.
//
// StringExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.CommandLine.Rendering
{
    internal static class StringExtensions
    {
        public static AnsiString ToAnsiString(this string value, AnsiStringStyle stringStyle) =>
            new AnsiString(value, stringStyle);

        public static string AsAnsiString(this string value, AnsiStringStyle stringStyle) =>
            new AnsiString(value, stringStyle);
    }
}
