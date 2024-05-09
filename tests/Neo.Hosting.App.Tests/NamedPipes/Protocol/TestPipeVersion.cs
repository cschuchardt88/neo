// Copyright (C) 2015-2024 The Neo Project.
//
// TestPipeVersion.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Hosting.App.NamedPipes.Protocol.Messages;
using Neo.Hosting.App.Tests.UTHelpers;
using Neo.Hosting.App.Tests.UTHelpers.Extensions;
using Xunit.Abstractions;

namespace Neo.Hosting.App.Tests.NamedPipes.Protocol
{
    public class TestPipeVersion
        (ITestOutputHelper testOutputHelper)
    {
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

        [Fact]
        public void IPipeMessage_FromArray()
        {
            var version1 = new PipeVersion();
            var expectedBytes = version1.ToArray();
            var expectedHexString = Convert.ToHexString(expectedBytes);

            var version2 = new PipeVersion();
            version2.FromArray(expectedBytes);

            var actualBytes = version2.ToArray();
            var actualHexString = Convert.ToHexString(actualBytes);

            var className = nameof(PipeVersion);
            var methodName = nameof(PipeVersion.FromArray);
            _testOutputHelper.LogDebug(className, methodName, actualHexString, expectedHexString);

            Assert.Equal(expectedBytes, actualBytes);
            Assert.Equal(version1.VersionNumber, version2.VersionNumber);
            Assert.Equal(version1.Platform, version2.Platform);
            Assert.Equal(version1.TimeStamp, version2.TimeStamp);
            Assert.Equal(version1.MachineName, version2.MachineName);
            Assert.Equal(version1.UserName, version2.UserName);
            Assert.Equal(version1.ProcessId, version2.ProcessId);
            Assert.Equal(version1.ProcessPath, version2.ProcessPath);
        }

        [Fact]
        public void IPipeMessage_ToArray_Stream()
        {
            var date = DateTime.UtcNow;
            var version1 = new PipeVersion()
            {
                TimeStamp = date,
            };
            var expectedBytes = version1.ToArray();
            var expectedHexString = Convert.ToHexString(expectedBytes);

            var version2 = new PipeVersion()
            {
                TimeStamp = date,
            };
            var actualBytes = version2.ToArray();
            var actualHexString = Convert.ToHexString(actualBytes);

            var className = nameof(PipeVersion);
            var methodName = nameof(PipeVersion.ToArray);
            _testOutputHelper.LogDebug(className, methodName, actualHexString, expectedHexString);

            Assert.Equal(expectedBytes, actualBytes);
            Assert.Equal(version1.VersionNumber, version2.VersionNumber);
            Assert.Equal(version1.Platform, version2.Platform);
            Assert.Equal(version1.TimeStamp, version2.TimeStamp);
            Assert.Equal(version1.MachineName, version2.MachineName);
            Assert.Equal(version1.UserName, version2.UserName);
            Assert.Equal(version1.ProcessId, version2.ProcessId);
            Assert.Equal(version1.ProcessPath, version2.ProcessPath);
        }
    }
}
