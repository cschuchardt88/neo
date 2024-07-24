// Copyright (C) 2015-2024 The Neo Project.
//
// FasterSnapshot.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using FASTER.core;
using Neo.IO;
using Neo.Persistence;
using Neo.Plugins.Storage.Faster;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Plugins.Storage
{
    public sealed class FasterSnapshot : ISnapshot
    {
        private readonly ICheckpointManager _snapshotCheckpointManager;
        private readonly FasterKV<byte[], byte[]> _snapshot;
        private readonly FasterStore _db;

        private readonly Guid _snapshotId;

        private readonly AsyncPool<ClientSession<byte[], byte[], byte[], byte[], Empty, ByteArrayFunctions>> _snapshotSessionPool;
        private readonly ConcurrentDictionary<byte[], byte[]?> _writeBatch;

        public FasterSnapshot(
            FasterStore store, string storePath, CheckpointSettings checkpointSettings, Guid snapshotId)
        {
            _db = store;
            _snapshotId = snapshotId;
            _writeBatch = new ConcurrentDictionary<byte[], byte[]?>(ByteArrayEqualityComparer.Default);

            _snapshotCheckpointManager = checkpointSettings.CheckpointManager;

            var snapshotLogSettings = new LogSettings()
            {
                LogDevice = new NullDevice(),
                ObjectLogDevice = new NullDevice(),
                ReadCopyOptions = new ReadCopyOptions(ReadCopyFrom.AllImmutable, ReadCopyTo.MainLog),
                ReadCacheSettings = new ReadCacheSettings()
                {
                    PageSizeBits = 9,
                    MemorySizeBits = 21,
                },
                PageSizeBits = 9,
                MemorySizeBits = 21,
                SegmentSizeBits = 21,
                MutableFraction = 0.3,
            };

            _snapshot = new
            (
                1 << 20,
                snapshotLogSettings,
                checkpointSettings,
                serializerSettings: new SerializerSettings<byte[], byte[]>()
                {
                    keySerializer = () => new ByteArrayBinaryObjectSerializer(),
                    valueSerializer = () => new ByteArrayBinaryObjectSerializer(),
                },
                comparer: new ByteArrayFasterEqualityComparer()
            );

            _snapshot.Recover(snapshotId);

            _snapshotSessionPool = new AsyncPool<ClientSession<byte[], byte[], byte[], byte[], Empty, ByteArrayFunctions>>
            (
                snapshotLogSettings.LogDevice.ThrottleLimit,
                () => _snapshot.For(new ByteArrayFunctions()).NewSession<ByteArrayFunctions>()
            );
        }

        public void Dispose()
        {
            _snapshot.Dispose();
            _snapshotSessionPool.Dispose();
            _snapshotCheckpointManager.Purge(_snapshotId);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            foreach (var kvp in _writeBatch)
            {
                if (kvp.Value is null)
                    _db.Delete(kvp.Key);
                else
                    _db.Put(kvp.Key, kvp.Value);
            }
        }

        public bool Contains(byte[] key)
        {
            return TryGet(key) != null;
        }

        public void Delete(byte[] key)
        {
            _writeBatch[key] = null;
        }

        public void Put(byte[] key, byte[] value)
        {
            _writeBatch[key] = value;
        }

        public IEnumerable<(byte[] Key, byte[] Value)> Seek(byte[] keyOrPrefix, SeekDirection direction)
        {
            if (_snapshotSessionPool.TryGet(out var session) == false)
                session = _snapshotSessionPool.GetAsync().AsTask().GetAwaiter().GetResult();

            var keyComparer = direction == SeekDirection.Forward ? ByteArrayComparer.Default : ByteArrayComparer.Reverse;
            var list = new List<(byte[] Key, byte[] Value)>();

            using var iter = session.Iterate(_snapshot.Log.TailAddress);
            while (iter.GetNext(out _))
            {
                var key = iter.GetKey();
                var value = iter.GetValue();

                if (keyOrPrefix?.Length > 0)
                {
                    if (keyComparer.Compare(key, keyOrPrefix) >= 0)
                        list.Add((key, value));
                }
            }

            _snapshotSessionPool.Return(session);

            return list.OrderBy(o => o.Key, keyComparer);
        }

        public byte[]? TryGet(byte[] key)
        {
            if (_snapshotSessionPool.TryGet(out var session) == false)
                session = _snapshotSessionPool.GetAsync().AsTask().GetAwaiter().GetResult();

            var (status, output) = session.Read(key);
            byte[]? value = null;

            if (status.Found)
                value = output;
            else if (status.IsPending && session.CompletePendingWithOutputs(out var iter, true, true))
            {
                using (iter)
                {
                    while (iter.Next())
                    {
                        if (iter.Current.Key.SequenceEqual(key))
                        {
                            value = iter.Current.Output;
                            break;
                        }
                    }
                }
            }

            _snapshotSessionPool.Return(session);
            return value;
        }
    }
}
