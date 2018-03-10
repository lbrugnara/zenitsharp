// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDecimal : FlInstance
    {
        private decimal rawValue;

        public FlDecimal(decimal value)
        {
            this.rawValue = value;
        }

        public override FlType Type => FlDecimalType.Instance;

        public override object RawValue => this.rawValue;

        public decimal Value { get => this.rawValue; set => this.rawValue = value; }

        public override FlObject Clone()
        {
            return new FlDecimal(this.rawValue);
        }
    }
}
