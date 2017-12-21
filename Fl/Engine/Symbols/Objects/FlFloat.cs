// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlFloat : FlInstance
    {
        private float _RawValue;

        public FlFloat(float value)
        {
            _RawValue = value;
        }

        public override FlType Type => FlFloatType.Instance;

        public override object RawValue => _RawValue;

        public float Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlFloat(_RawValue);
        }
    }
}
