// Copyright (C) 2015-2024 The Neo Project.
//
// NullExceptionFilter.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Hosting.App.Extensions;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Neo.Hosting.App.Handlers
{
    internal static class NullExceptionFilter
    {
        internal static void Handler(Exception exception, InvocationContext context)
        {
#if DEBUG
            if (exception is not OperationCanceledException)
            {
                context.Console.WriteLine(string.Empty);
                context.Console.ErrorMessage(exception);
            }
#endif

            context.ExitCode = exception.HResult;
        }
    }
}
