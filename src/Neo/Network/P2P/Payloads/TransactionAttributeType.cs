// Copyright (C) 2015-2025 The Neo Project.
//
// TransactionAttributeType.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the repository
// or https://opensource.org/license/mit for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.IO.Caching;

namespace Neo.Network.P2P.Payloads
{
    /// <summary>
    /// Represents the type of a <see cref="TransactionAttribute"/>.
    /// </summary>
    public enum TransactionAttributeType : byte
    {
        /// <summary>
        /// Indicates that the transaction is of high priority.
        /// </summary>
        [ReflectionCache(typeof(HighPriorityAttribute))]
        HighPriority = 0x01,

        /// <summary>
        /// Indicates that the transaction is an oracle response.
        /// </summary>
        [ReflectionCache(typeof(OracleResponse))]
        OracleResponse = 0x11,

        /// <summary>
        /// Indicates that the transaction is not valid before <see cref="NotValidBefore.Height"/>.
        /// </summary>
        [ReflectionCache(typeof(NotValidBefore))]
        NotValidBefore = 0x20,

        /// <summary>
        /// Indicates that the transaction conflicts with <see cref="Conflicts.Hash"/>.
        /// </summary>
        [ReflectionCache(typeof(Conflicts))]
        Conflicts = 0x21
    }
}
