// Copyright (C) 2015-2025 The Neo Project.
//
// Program.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.CLI;

namespace Neo
{
    static class Program
    {
        private static readonly string s_blockHex =
            "000000000000000000000000000000000000000000000000000000000000000000000000" +
            "6c23be5d32679baa9c5c2aa0d329fd2a2441d7875d0f34d42f58f70428fbbbb9493ed0e58f01" +
            "00000000000000000000000000000000000000000000000000000000000000000000000" +
            "10001110100000000000000000000000000000000000000000000000000010000000000" +
            "00000000000000000000000000000001000112010000";
        static void Main(string[] args)
        {
            byte[] j = [2, 3, 4];
            var mainService = new MainService();
            mainService.Run(args);
        }
    }
}
