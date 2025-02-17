// Copyright (C) 2015-2025 The Neo Project.
//
// JsonStringECPointConverter.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Build.Core.Exceptions;
using Neo.Build.Core.Extensions;
using Neo.Cryptography.ECC;
using Neo.Extensions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neo.Build.Core.Json.Converters
{
    public class JsonStringECPointConverter : JsonConverter<ECPoint>
    {
        public override ECPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new NeoBuildInvalidECPointFormatException();

            var valueString = reader.GetString();

            if (string.IsNullOrEmpty(valueString))
                throw new NeoBuildInvalidECPointFormatException();

            var valueBytes = valueString.StartsWith("0x") ?
                StringConverter.FromHexString(valueString[2..]) :
                StringConverter.FromHexString(valueString);

            return ECPoint.FromBytes(valueBytes, ECCurve.Secp256r1);
        }

        public override void Write(Utf8JsonWriter writer, ECPoint value, JsonSerializerOptions options)
        {
            var valueBytes = value.ToArray();
            writer.WriteStringValue(ByteConverter.ToHexString(valueBytes));
        }
    }
}
