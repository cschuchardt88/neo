// Copyright (C) 2015-2024 The Neo Project.
//
// AssemblyExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Reflection;

namespace Neo.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetVersion(this Assembly assembly)
        {
            return assembly.GetName().Version!.ToString(3);
        }

        public static int GetVersionNumber(this Assembly assembly)
        {
            var version = assembly.GetName().Version;

            if (version is null) return 0;

            return version.Major * 1000 + version.Minor * 100 + version.Build * 10 + version.Revision;
        }
    }
}
