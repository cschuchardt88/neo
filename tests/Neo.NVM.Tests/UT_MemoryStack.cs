// Copyright (C) 2015-2025 The Neo Project.
//
// UT_MemoryStack.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.NVM.Collections;

namespace Neo.NVM.Tests
{
    [TestClass]
    public class UT_MemoryStack
    {
        [TestMethod]
        public void TestPush()
        {
            var stack = new MemoryStack();

            stack.Push(new());
            stack.Push(new());

            Assert.AreEqual(2, stack.Length);
        }

        [TestMethod]
        public void TestPop()
        {
            var stack = new MemoryStack();

            stack.Push(new() { Tag = "1" });
            stack.Push(new() { Tag = "2" });

            Assert.AreEqual(2, stack.Length);

            var actualTopStack = stack.Pop();

            Assert.AreEqual(1, stack.Length);
            Assert.IsNotNull(actualTopStack);
            Assert.AreEqual("2", actualTopStack.Tag);

            actualTopStack = stack.Pop();

            Assert.AreEqual(0, stack.Length);
            Assert.IsNotNull(actualTopStack);
            Assert.AreEqual("1", actualTopStack.Tag);
        }

        [TestMethod]
        public void TestPeek()
        {
            var stack = new MemoryStack();

            stack.Push(new() { Tag = "1" });
            stack.Push(new() { Tag = "2" });

            Assert.AreEqual(2, stack.Length);

            var actualTopStack = stack.Peek(0);

            Assert.IsNotNull(actualTopStack);
            Assert.AreEqual("2", actualTopStack.Tag);

            actualTopStack = stack.Peek(1);

            Assert.IsNotNull(actualTopStack);
            Assert.AreEqual("1", actualTopStack.Tag);
        }
    }
}
