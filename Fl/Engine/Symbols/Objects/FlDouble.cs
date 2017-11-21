// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols
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
    }
}
