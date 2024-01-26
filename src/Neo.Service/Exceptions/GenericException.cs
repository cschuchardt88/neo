// Copyright (C) 2015-2024 The Neo Project.
//
// GenericException.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.IO;
using System;
using System.IO;

namespace Neo.Service.Exceptions
{
    public class GenericException : ISerializable
    {
        public int Code { get; private set; }
        public string? Message { get; private set; }
        public string? StackTrace { get; private set; }

        public static GenericException Create(Exception exception) =>
            new()
            {
                Code = exception.HResult,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
            };

        public int Size =>
            sizeof(int) +           // Code
            Message.GetVarSize();   // Message

        public void Deserialize(ref MemoryReader reader)
        {
            Code = reader.ReadInt32();
            Message = reader.ReadVarString();
            StackTrace = reader.ReadVarString();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteVarInt(Code);
            writer.WriteVarString(Message);
            writer.WriteVarString(StackTrace);
        }
    }
}
