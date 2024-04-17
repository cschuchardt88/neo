// Copyright (C) 2015-2024 The Neo Project.
//
// NeoSystemService.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Akka.Actor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neo.Hosting.App.Configuration;
using Neo.Network.P2P;
using Neo.Persistence;
using Neo.Plugins;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.Hosting.App.Hosting
{
    internal sealed class NeoSystemService : IHostedService, IDisposable
    {
        public bool IsRunning { get; private set; }
        public NeoSystem? NeoSystem => _neoSystem;

        private readonly ProtocolSettings _protocolSettings;
        private readonly SystemOptions _systemOptions;

        private NeoSystem? _neoSystem;
        private LocalNode? _localNode;

        public NeoSystemService(
            IConfiguration config,
            IOptions<SystemOptions> systemOptions)
        {
            _protocolSettings = ProtocolSettings.Load(config.GetRequiredSection("ProtocolConfiguration"));
            _systemOptions = systemOptions.Value;

            Plugin.LoadPlugins();
        }

        public void Dispose()
        {
            StopAsync(CancellationToken.None).Wait();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string? storagePath = null;
            if (string.IsNullOrEmpty(_systemOptions.Storage.Path) == false)
            {
                storagePath = string.Format(_systemOptions.Storage.Path, _protocolSettings.Network);
                if (Directory.Exists(storagePath) == false)
                {
                    if (Path.IsPathFullyQualified(storagePath) == false)
                        storagePath = Path.Combine(AppContext.BaseDirectory, storagePath);
                }
            }

            if (StoreFactory.GetStoreProvider(_systemOptions.Storage.Engine) is null)
                throw new DllNotFoundException($"Plugin '{_systemOptions.Storage.Engine}.dll' can't be found.");

            _neoSystem ??= new(_protocolSettings, _systemOptions.Storage.Engine, storagePath);
            _localNode ??= await _neoSystem.LocalNode.Ask<LocalNode>(new LocalNode.GetInstance(), cancellationToken);
            IsRunning = true;
        }

        public void StartNode()
        {
            if (_neoSystem is null)
                throw new NullReferenceException("NeoSystem");

            _neoSystem.StartNode(new()
            {
                Tcp = new(IPAddress.Parse(_systemOptions.P2P.Listen), _systemOptions.P2P.Port),
                MinDesiredConnections = _systemOptions.P2P.MinDesiredConnections,
                MaxConnections = _systemOptions.P2P.MaxConnections,
                MaxConnectionsPerAddress = _systemOptions.P2P.MaxConnectionsPerAddress,
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _neoSystem?.Dispose();
            IsRunning = false;

            return Task.CompletedTask;
        }
    }
}
