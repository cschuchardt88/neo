using Neo.IO;
using Neo.VM;
using Neo.VM.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Array = Neo.VM.Types.Array;
using Boolean = Neo.VM.Types.Boolean;

namespace Neo.SmartContract
{
    internal static class StackItemSerializer
    {
        public static StackItem Deserialize(byte[] data, uint maxArraySize, uint maxItemSize)
        {
            using MemoryStream ms = new MemoryStream(data, false);
            using BinaryReader reader = new BinaryReader(ms);
            return Deserialize(reader, maxArraySize, maxItemSize);
        }

        public static unsafe StackItem Deserialize(ReadOnlySpan<byte> data, uint maxArraySize, uint maxItemSize)
        {
            if (data.IsEmpty) throw new FormatException();
            fixed (byte* pointer = data)
            {
                using UnmanagedMemoryStream ms = new UnmanagedMemoryStream(pointer, data.Length);
                using BinaryReader reader = new BinaryReader(ms);
                return Deserialize(reader, maxArraySize, maxItemSize);
            }
        }

        private static StackItem Deserialize(BinaryReader reader, uint maxArraySize, uint maxItemSize)
        {
            Stack<StackItem> deserialized = new Stack<StackItem>();
            int undeserialized = 1;
            while (undeserialized-- > 0)
            {
                StackItemType type = (StackItemType)reader.ReadByte();
                switch (type)
                {
                    case StackItemType.ByteArray:
                        deserialized.Push(new ByteArray(reader.ReadVarBytes((int)maxItemSize)));
                        break;
                    case StackItemType.Boolean:
                        deserialized.Push(new Boolean(reader.ReadBoolean()));
                        break;
                    case StackItemType.Integer:
                        deserialized.Push(new Integer(new BigInteger(reader.ReadVarBytes(ExecutionEngine.MaxSizeForBigInteger))));
                        break;
                    case StackItemType.Array:
                    case StackItemType.Struct:
                        {
                            int count = (int)reader.ReadVarInt(maxArraySize);
                            deserialized.Push(new ContainerPlaceholder
                            {
                                Type = type,
                                ElementCount = count
                            });
                            undeserialized += count;
                        }
                        break;
                    case StackItemType.Map:
                        {
                            int count = (int)reader.ReadVarInt(maxArraySize);
                            deserialized.Push(new ContainerPlaceholder
                            {
                                Type = type,
                                ElementCount = count
                            });
                            undeserialized += count * 2;
                        }
                        break;
                    case StackItemType.Null:
                        deserialized.Push(StackItem.Null);
                        break;
                    default:
                        throw new FormatException();
                }
            }
            Stack<StackItem> stack_temp = new Stack<StackItem>();
            while (deserialized.Count > 0)
            {
                StackItem item = deserialized.Pop();
                if (item is ContainerPlaceholder placeholder)
                {
                    switch (placeholder.Type)
                    {
                        case StackItemType.Array:
                            Array array = new Array();
                            for (int i = 0; i < placeholder.ElementCount; i++)
                                array.Add(stack_temp.Pop());
                            item = array;
                            break;
                        case StackItemType.Struct:
                            Struct @struct = new Struct();
                            for (int i = 0; i < placeholder.ElementCount; i++)
                                @struct.Add(stack_temp.Pop());
                            item = @struct;
                            break;
                        case StackItemType.Map:
                            Map map = new Map();
                            for (int i = 0; i < placeholder.ElementCount; i++)
                            {
                                StackItem key = stack_temp.Pop();
                                StackItem value = stack_temp.Pop();
                                map.Add((PrimitiveType)key, value);
                            }
                            item = map;
                            break;
                    }
                }
                stack_temp.Push(item);
            }
            return stack_temp.Peek();
        }

        public static byte[] Serialize(StackItem item)
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(ms);
            Serialize(item, writer);
            writer.Flush();
            return ms.ToArray();
        }

        private static void Serialize(StackItem item, BinaryWriter writer)
        {
            List<StackItem> serialized = new List<StackItem>();
            Stack<StackItem> unserialized = new Stack<StackItem>();
            unserialized.Push(item);
            while (unserialized.Count > 0)
            {
                item = unserialized.Pop();
                switch (item)
                {
                    case ByteArray bytes:
                        writer.Write((byte)StackItemType.ByteArray);
                        writer.WriteVarBytes(bytes.ToByteArray());
                        break;
                    case Boolean _:
                        writer.Write((byte)StackItemType.Boolean);
                        writer.Write(item.ToBoolean());
                        break;
                    case Integer integer:
                        writer.Write((byte)StackItemType.Integer);
                        writer.WriteVarBytes(integer.ToByteArray());
                        break;
                    case InteropInterface _:
                        throw new NotSupportedException();
                    case Array array:
                        if (serialized.Any(p => ReferenceEquals(p, array)))
                            throw new NotSupportedException();
                        serialized.Add(array);
                        if (array is Struct)
                            writer.Write((byte)StackItemType.Struct);
                        else
                            writer.Write((byte)StackItemType.Array);
                        writer.WriteVarInt(array.Count);
                        for (int i = array.Count - 1; i >= 0; i--)
                            unserialized.Push(array[i]);
                        break;
                    case Map map:
                        if (serialized.Any(p => ReferenceEquals(p, map)))
                            throw new NotSupportedException();
                        serialized.Add(map);
                        writer.Write((byte)StackItemType.Map);
                        writer.WriteVarInt(map.Count);
                        foreach (var pair in map.Reverse())
                        {
                            unserialized.Push(pair.Value);
                            unserialized.Push(pair.Key);
                        }
                        break;
                    case Null _:
                        writer.Write((byte)StackItemType.Null);
                        break;
                }
            }
        }
    }
}