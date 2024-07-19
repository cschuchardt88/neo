// Copyright (C) 2015-2024 The Neo Project.
//
// DefaultPromptCommand.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.Logging;
using Neo.Hosting.App.Helpers;
using Neo.Hosting.App.Host.Service;
using System;
using System.CommandLine;

namespace Neo.Hosting.App.CommandLine.Prompt
{
    internal sealed class DefaultPromptCommand : Command
    {
        private static string? s_executablePath;

        public DefaultPromptCommand(
            ILoggerFactory loggerFactory,
            NamedPipeClientService clientService) : base(ExecutableName, $"Your are connected to {ExecutablePath}")
        {
            var exportCommand = new ExportCommand();
            var helpCommand = new HelpCommand();
            var quitCommand = new QuitCommand();
            var showCommand = new ShowCommand(loggerFactory, clientService);
            var walletCommand = new WalletCommand();
            var pluginCommand = new PluginCommand();
            var contractCommand = new ContractCommand();
            var candidateCommand = new CandidateCommand();
            var nep17Command = new Nep17Command();

            AddCommand(candidateCommand);
            AddCommand(contractCommand);
            AddCommand(exportCommand);
            AddCommand(nep17Command);
            AddCommand(pluginCommand);
            AddCommand(walletCommand);
            AddCommand(showCommand);
            AddCommand(helpCommand);
            AddCommand(quitCommand);
        }

        public static string ExecutableName => $"{Environment.UserName}@{Environment.MachineName}:~$";

        public static string ExecutablePath =>
            s_executablePath ??= $"{EnvironmentUtility.AddOrGetServicePipeName()}";
    }
}
