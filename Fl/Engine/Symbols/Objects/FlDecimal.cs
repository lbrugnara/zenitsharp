// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDecimal : FlInstance
    {
        private decimal _RawValue;

        public FlDecimal(decimal value)
        {
            _RawValue = value;
        }

        public override FlType Type => FlDecimalType.Instance;

        public override object RawValue => _RawValue;

        public decimal Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlDecimal(_RawValue);
        }
    }
}
