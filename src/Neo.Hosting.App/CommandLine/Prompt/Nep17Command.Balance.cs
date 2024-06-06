// Copyright (C) 2015-2024 The Neo Project.
//
// Nep17Command.Balance.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Neo.Hosting.App.CommandLine.Prompt
{
    internal partial class Nep17Command
    {
        internal sealed class BalanceCommand : Command
        {
            public BalanceCommand() : base("balanceof", "Get a balance for an address")
            {
                var scriptHashArgument = new Argument<string>("SCRIPTHASH", "160-bit hash (hex)");
                var addressHashArgument = new Argument<string>("ADDRESS", "160-bit hash (hex) or NEO address");

                AddArgument(scriptHashArgument);
                AddArgument(addressHashArgument);

                this.SetHandler(async context => await new Handler().InvokeAsync(context));
            }

            internal sealed new class Handler : ICommandHandler
            {
                public Task<int> InvokeAsync(InvocationContext context)
                {
                    return Task.FromResult(0);
                }

                public int Invoke(InvocationContext context)
                {
                    throw new NotImplementedException();
                }

            }
        }
    }
}
