// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols
{
    public class FlInteger : FlObject
    {
        private int _RawValue;

        public FlInteger(int value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => IntegerType.Value;

        public override object RawValue => _RawValue;

        public int Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlInteger(_RawValue);
        }
    }
}
