// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlDouble : FlInstance
    {
        private double _RawValue;

        public FlDouble(double value)
        {
            _RawValue = value;
        }

        public override FlType Type => FlDoubleType.Instance;

        public override object RawValue => _RawValue;

        public double Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlDouble(_RawValue);
        }
    }
}
