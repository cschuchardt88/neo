// Copyright (C) 2015-2024 The Neo Project.
//
// CountModel.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.Plugins.RestServer.Models
{
    internal class CountModel
    {
        /// <summary>
        /// The count of how many objects.
        /// </summary>
        /// <example>378</example>
        public int Count { get; set; }
    }
}