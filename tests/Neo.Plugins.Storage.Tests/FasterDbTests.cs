// Copyright (C) 2015-2024 The Neo Project.
//
// FasterDbTests.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Extensions;
using Neo.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo.Plugins.Storage.Tests
{
    [TestClass]
    public class FasterDbTests
    {
        [TestMethod]
        public void Test_LinkedList_Seek()
        {
            var ll = new LinkedList<byte[]>();

            // Note: LinkedList have to be in order for seeking to work.
            var first = ll.AddFirst([0x00, 0x00]);
            var second = ll.AddAfter(first, [0x00, 0x01]);
            var third = ll.AddAfter(second, [0x01, 0x02]);

            var fKeys = ll.Seek([0x00, 0x01], SeekDirection.Forward);
            var bKeys = ll.Seek([0x00, 0x01], SeekDirection.Backward);

            var actualSeekForward = fKeys.ToArray();
            var actualSeekBackward = bKeys.ToArray();

            Assert.AreEqual(actualSeekForward.Length, 2);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x01 }, actualSeekForward[0]);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02 }, actualSeekForward[1]);

            Assert.AreEqual(actualSeekBackward.Length, 2);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x01 }, actualSeekBackward[0]);
            CollectionAssert.AreEqual(new byte[] { 0x00, 0x00 }, actualSeekBackward[1]);
        }
    }
}
