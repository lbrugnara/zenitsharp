// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols
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
    }
}
