// Copyright (C) 2015-2025 The Neo Project.
//
// JsonInvalidFormatException.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Build.Exceptions.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Neo.Build.Exceptions.Json
{
    internal class JsonInvalidFormatException(
            [AllowNull] string? value) : NeoBuildException(), IInvalidJsonFormatException
    {
        /// <inheritdoc />
        public override int HResult =>
            NeoBuildErrorCodes.General.InvalidJsonFormat;

        public virtual string Value => value ?? "null";
    }
}
