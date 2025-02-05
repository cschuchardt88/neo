// Copyright (C) 2015-2025 The Neo Project.
//
// NotCondition.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the repository
// or https://opensource.org/license/mit for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Extensions;
using Neo.IO;
using Neo.Json;
using Neo.SmartContract;
using Neo.VM;
using Neo.VM.Types;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Neo.Network.P2P.Payloads.Conditions
{
    /// <summary>
    /// Reverse another condition.
    /// </summary>
    public class NotCondition : WitnessCondition, IEquatable<NotCondition>
    {
        /// <summary>
        /// The expression of the condition to be reversed.
        /// </summary>
        public WitnessCondition Expression;

        public override int Size => base.Size + Expression.Size;
        public override WitnessConditionType Type => WitnessConditionType.Not;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(NotCondition other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (other is null) return false;
            return
                Type == other.Type &&
                Expression.Equals(other.Expression);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is NotCondition nc && Equals(nc);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Expression.GetHashCode());
        }

        protected override void DeserializeWithoutType(ref MemoryReader reader, int maxNestDepth)
        {
            if (maxNestDepth <= 0) throw new FormatException();
            Expression = DeserializeFrom(ref reader, maxNestDepth - 1);
        }

        public override bool Match(ApplicationEngine engine)
        {
            return !Expression.Match(engine);
        }

        protected override void SerializeWithoutType(BinaryWriter writer)
        {
            writer.Write(Expression);
        }

        private protected override void ParseJson(JObject json, int maxNestDepth)
        {
            if (maxNestDepth <= 0) throw new FormatException();
            Expression = FromJson((JObject)json["expression"], maxNestDepth - 1);
        }

        public override JObject ToJson()
        {
            JObject json = base.ToJson();
            json["expression"] = Expression.ToJson();
            return json;
        }

        public override StackItem ToStackItem(IReferenceCounter referenceCounter)
        {
            var result = (VM.Types.Array)base.ToStackItem(referenceCounter);
            result.Add(Expression.ToStackItem(referenceCounter));
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(NotCondition left, NotCondition right)
        {
            if (left is null || right is null)
                return Equals(left, right);

            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(NotCondition left, NotCondition right)
        {
            if (left is null || right is null)
                return !Equals(left, right);

            return !left.Equals(right);
        }
    }
}
