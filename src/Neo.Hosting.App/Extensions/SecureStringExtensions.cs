// Copyright (C) 2015-2024 The Neo Project.
//
// SecureStringExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Neo.Hosting.App.Extensions
{
    internal static class SecureStringExtensions
    {
        public static string? GetClearText(this SecureString secureString)
        {
            ArgumentNullException.ThrowIfNull(secureString, nameof(secureString));

            var unmanagedString = nint.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
