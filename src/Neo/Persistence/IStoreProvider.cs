// Copyright (C) 2015-2025 The Neo Project.
//
// IStoreProvider.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

#nullable enable

namespace Neo.Persistence
{
    /// <summary>
    /// A provider used to create <see cref="IStore"/> instances.
    /// </summary>
    public interface IStoreProvider
    {
        /// <summary>
        /// Gets the name of the <see cref="IStoreProvider"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="IStore"/> interface.
        /// </summary>
        /// <param name="path">The path of the database.</param>
        /// <returns>The created <see cref="IStore"/> instance.</returns>
        IStore GetStore(string path);
    }
}
