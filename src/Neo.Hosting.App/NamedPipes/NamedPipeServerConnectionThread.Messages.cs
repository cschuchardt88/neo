// Copyright (C) 2015-2024 The Neo Project.
//
// NamedPipeServerConnectionThread.Messages.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Hosting.App.NamedPipes.Protocol;
using Neo.Hosting.App.NamedPipes.Protocol.Messages;
using Neo.Hosting.App.NamedPipes.Protocol.Payloads;
using Neo.Network.P2P.Payloads;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Neo.Hosting.App.NamedPipes
{
    internal partial class NamedPipeServerConnectionThread
    {
        private PipeMessage CreateErrorResponse(int requestId, Exception exception)
        {
            var error = new PipeExceptionPayload(exception);
            return PipeMessage.Create(requestId, PipeCommand.Exception, error);
        }

        private async Task OnRequestMessageAsync(PipeMessage message)
        {
            var responseMessage = message.Command switch
            {
                PipeCommand.GetVersion => OnVersion(message),
                PipeCommand.GetBlock => OnBlock(message),
                PipeCommand.GetTransaction => OnTransaction(message),
                _ => CreateErrorResponse(message.RequestId, new InvalidDataException()),
            };

            await WriteAsync(responseMessage);
        }

        private PipeMessage OnVersion(PipeMessage message) =>
            PipeMessage.Create(message.RequestId, PipeCommand.Version, new PipeVersionPayload());

        private PipeMessage OnBlock(PipeMessage message)
        {
            if (message.Payload is not PipeUnmanagedPayload<uint> blockIndex)
                return CreateErrorResponse(message.RequestId, new InvalidDataException());

            var block = _neoSystemService.GetBlock(blockIndex.Value);
            var payload = new PipeSerializablePayload<Block>() { Value = block };

            return PipeMessage.Create(message.RequestId, PipeCommand.Block, payload);
        }

        private PipeMessage OnTransaction(PipeMessage message)
        {
            if (message.Payload is not PipeSerializablePayload<UInt256> transactionHash)
                return CreateErrorResponse(message.RequestId, new InvalidDataException());

            var transaction = _neoSystemService.GetTransaction(transactionHash.Value);
            var payload = new PipeSerializablePayload<Transaction>() { Value = transaction };

            return PipeMessage.Create(message.RequestId, PipeCommand.Transaction, payload);
        }
    }
}
