// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDecimal : FlObject
    {
        private decimal _RawValue;

        public FlDecimal(decimal value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => DecimalType.Value;

        public override object RawValue => _RawValue;

        public decimal Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlDecimal(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == DecimalType.Value)
            {
                return this.Clone();
            }
            else if (type == IntegerType.Value)
            {
                return new FlInteger((int)_RawValue);
            }
            else if (type == DoubleType.Value) 
            {
                return new FlDouble((double)_RawValue);
            }
            else if (type == StringType.Value)
            {
                return new FlString(_RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}
