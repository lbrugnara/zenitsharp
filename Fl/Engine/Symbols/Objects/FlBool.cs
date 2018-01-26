// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlBool : FlInstance
    {
        private bool _RawValue;

        public FlBool(bool value)
        {
            _RawValue = value;
        }

        public override FlType Type => FlBoolType.Instance;

        public override object RawValue => _RawValue;

        public bool Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlBool(_RawValue);
        }
    }
}
