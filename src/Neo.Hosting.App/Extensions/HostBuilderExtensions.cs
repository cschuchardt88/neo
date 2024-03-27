// Copyright (C) 2015-2024 The Neo Project.
//
// HostBuilderExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Neo.Hosting.App.Factories;
using Neo.Hosting.App.NamedPipes;
using Neo.Wallets;
using System;
using System.IO;
using System.Security;

namespace Neo.Hosting.App.Extensions
{
    internal static class HostBuilderExtensions
    {
        public static IHostBuilder UseNamedPipes(this IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureServices(services =>
            {
                services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
                services.AddSingleton<NamedPipeTransportFactory>();
            });

        public static IHostBuilder UseNamedPipes(this IHostBuilder hostBuilder, Action<NamedPipeTransportOptions> configureOptions) =>
            hostBuilder.UseNamedPipes().ConfigureServices(services => services.Configure(configureOptions));

        public static void AddWalletJsonFile(this IServiceCollection services, string path, SecureString password, ProtocolSettings? protocolSettings = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            var fileName = Path.GetFileName(path);

            if (File.Exists(path) == false)
                throw new FileNotFoundException(fileName);

            var wallet = Wallet.Open(path, password.GetValue(), protocolSettings ?? ProtocolSettings.Default) ??
                throw new FileLoadException("Wallet information is correct.", fileName);

            services.AddSingleton(wallet);
        }
    }
}