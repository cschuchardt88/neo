// Copyright (C) 2015-2024 The Neo Project.
//
// UnitTest1.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Cryptography;
using Neo.IO;
using Neo.Plugins.AutoBackup.Tests.Helpers;
using System;
using System.IO;

namespace Neo.Plugins.AutoBackup.Tests
{
    [TestClass]
    public class UT_ArchiveFile
    {
        [TestInitialize]
        public void Initialize()
        {
            if (File.Exists("Test.zip"))
                File.Delete("Test.zip");
        }

        [TestMethod]
        public void ArchiveFile_And_ReadWriteBlock()
        {
            var expectedBlock = UT_RandomGenerator.RandomBlock(0);
            var expectedCrc32 = Crc32.Compute(expectedBlock.ToArray());

            using (ArchiveFile archive = new("Test.zip", true))
            {
                archive.WriteBlockEntry(expectedBlock, 0xDEAD_C0DE);
            }

            using (ArchiveFile archive = new("Test.zip", false))
            {
                var actualBlock = archive.ReadBlockEntry(0);
                Assert.AreEqual(0xDEAD_C0DE, actualBlock.Network);
                Assert.AreEqual(expectedCrc32, actualBlock.Checksum);
                Assert.AreEqual(expectedBlock.Index, actualBlock.Block.Index);
                Assert.AreEqual(expectedBlock.Hash, actualBlock.Block.Hash);
            }
        }
    }
}
