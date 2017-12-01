// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class FlTuple : FlObject
    {
        private List<FlObject> _RawValue;

        public FlTuple(List<FlObject> value = null)
        {
            _RawValue = value ?? new List<FlObject>();
        }

        public override ObjectType ObjectType => TupleType.Value;

        public override object RawValue => _RawValue;

        public List<FlObject> Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlTuple(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == TupleType.Value)
            {
                return this.Clone();
            }
            else if (type == StringType.Value)
            {
                return new FlString(this.RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public override string ToString()
        {
            return $"({string.Join(", ", _RawValue.Select(o => o.ToString()))}) ({ObjectType})";
        }

        public override string ToDebugStr()
        {
            return $"({string.Join(", ", _RawValue.Select(o => o.ToDebugStr()))}) ({ObjectType})";
        }

        public override FlObject this[FlObject index]
        {
            get
            {
                if (index.ObjectType != IntegerType.Value)
                {
                    throw new SymbolException($"{ObjectType} expects indexer to be of type {IntegerType.Value}");
                }
                return _RawValue.ElementAtOrDefault((index as FlInteger).Value) ?? FlNull.Value;
            }
            set
            {
                if (index.ObjectType != IntegerType.Value)
                {
                    throw new SymbolException($"{ObjectType} expects indexer to be of type {IntegerType.Value}");
                }
                _RawValue[(index as FlInteger).Value] = value ?? FlNull.Value;
            }
        }

        #region Arithmetics Operators

        public override void AddAndAssign(FlObject n)
        {
            this.Value.Add(n);
        }

        #endregion
    }
}
