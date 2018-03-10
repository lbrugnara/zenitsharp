// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDouble : FlInstance
    {
        private double rawValue;

        public FlDouble(double value)
        {
            this.rawValue = value;
        }

        public override FlType Type => FlDoubleType.Instance;

        public override object RawValue => this.rawValue;

        public double Value { get => this.rawValue; set => this.rawValue = value; }

        public override FlObject Clone()
        {
            return new FlDouble(this.rawValue);
        }
    }
}
