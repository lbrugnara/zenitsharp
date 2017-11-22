// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDouble : FlObject
    {
        private double _RawValue;

        public FlDouble(double value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => DoubleType.Value;

        public override object RawValue => _RawValue;

        public double Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlDouble(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == DoubleType.Value) 
            {
                return this.Clone();
            }
            else if (type == IntegerType.Value)
            {
                return new FlInteger((int)_RawValue);
            }
            else if (type == DecimalType.Value)
            {
                return new FlDecimal((decimal)_RawValue);
            }
            else if (type == StringType.Value)
            {
                return new FlString(_RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}
