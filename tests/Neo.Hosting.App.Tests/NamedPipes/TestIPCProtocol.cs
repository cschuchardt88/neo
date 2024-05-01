// Copyright (C) 2015-2024 The Neo Project.
//
// TestIPCProtocol.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Akka.Util;
using Neo.Hosting.App.Extensions;
using Neo.Hosting.App.Factories;
using Neo.Hosting.App.NamedPipes.Protocol;
using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Neo.Hosting.App.Tests.NamedPipes
{
    public class TestIPCProtocol
        (ITestOutputHelper testOutputHelper)
    {
        private static readonly byte[] s_testData = Encoding.UTF8.GetBytes("Hello world");

        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

        [Fact]
        public async Task PipeVersion_CopyToAsync()
        {
            var version = new PipeVersion();
            var expectedBytes = version.ToArray();
            var expectedHexString = Convert.ToHexString(expectedBytes);

            using var ms = new MemoryStream();
            await version.CopyToAsync(ms).DefaultTimeout();

            var actualBytes = ms.ToArray();
            var actualHexString = Convert.ToHexString(actualBytes);

            var className = nameof(PipeVersion);
            var methodName = nameof(PipeVersion.CopyToAsync);
            _testOutputHelper.WriteLine(nameof(Debug).PadCenter(17, '-'));
            _testOutputHelper.WriteLine($"    Class: {className}");
            _testOutputHelper.WriteLine($"   Method: {methodName}");

            _testOutputHelper.WriteLine(nameof(Result).PadCenter(17, '-'));
            _testOutputHelper.WriteLine($"   Actual: {actualHexString}");
            _testOutputHelper.WriteLine($" Expected: {expectedHexString}");
            _testOutputHelper.WriteLine($"-----------------");

            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public async Task BidirectionalStream_ServerReadsDataAndCompletes_GracefullyClosed()
        {
            await using var connectionListener = await NamedPipeTransportFactory.CreateConnectionListener();
            var clientConnection = NamedPipeTransportFactory.CreateClientStream(connectionListener.EndPoint);

            // Client connecting
            await clientConnection.ConnectAsync().DefaultTimeout();

            // Server accepting stream
            var serverConnectionTask = connectionListener.AcceptAsync();

            // Client sending data
            var writeTask = clientConnection.WriteAsync(s_testData);

            var serverConnection = await serverConnectionTask.DefaultTimeout();
            await writeTask.DefaultTimeout();

            // Server reading data
            var readResult = await serverConnection!.Transport!.Input.ReadAtLeastAsync(s_testData.Length).DefaultTimeout();
            serverConnection.Transport.Input.AdvanceTo(readResult.Buffer.End);

            clientConnection.Close();

            readResult = await serverConnection.Transport.Input.ReadAsync();
            Assert.True(readResult.IsCompleted);

            // Server completing input and output
            await serverConnection.Transport.Input.CompleteAsync();
            await serverConnection.Transport.Output.CompleteAsync();

            // Server disposing connection
            await serverConnection.DisposeAsync();
        }
    }
}
