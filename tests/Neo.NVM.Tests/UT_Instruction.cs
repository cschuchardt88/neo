// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Instruction.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Neo.NVM.Tests
{
    [TestClass]
    public sealed class UT_Instruction
    {
        [TestMethod]
        public void TestMethod1()
        {
            ReadOnlyMemory<byte> expectedScript = new byte[] { 0x00, 0x7F, 0x00, 0x05 };

            var actualInstruction = new Instruction(expectedScript);
            var actualInstructions = actualInstruction.ToArray();

            Assert.AreEqual(OpCode.PUSHINT8, actualInstructions[0].Code);
            Assert.AreEqual(sbyte.MaxValue, actualInstructions[0].AsToken<sbyte>());

            Assert.AreEqual(OpCode.PUSHINT8, actualInstructions[1].Code);
            Assert.AreEqual(5, actualInstructions[1].AsToken<sbyte>());
        }
    }
}
