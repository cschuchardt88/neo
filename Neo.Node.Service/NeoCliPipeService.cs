// Copyright (C) 2015-2024 The Neo Project.
//
// NeoCliPipeService.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Pipes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.Node.Service
{
    public class NeoCliPipeService : BackgroundService
    {
        private readonly ILogger<NeoCliPipeService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        private NamedPipeServerStream? _neoPipeServer;
        private int _pipeServerThreadId = -1;

        public NeoCliPipeService(
            ILogger<NeoCliPipeService> logger)
        {
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                WriteIndented = false,
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _neoPipeServer = new("neoclipipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
                _pipeServerThreadId = Environment.CurrentManagedThreadId;

                while (stoppingToken.IsCancellationRequested == false)
                {
                    await _neoPipeServer!.WaitForConnectionAsync(stoppingToken);
                    var command = await JsonSerializer.DeserializeAsync<PipeCommand>(_neoPipeServer, _serializerOptions, stoppingToken);

                    if (command == null)
                    {
                        _neoPipeServer.Disconnect();
                        continue;
                    }


                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("{Exception}", ex!.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
