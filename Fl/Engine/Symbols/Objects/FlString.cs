// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols
{
    public class FlString : FlObject
    {
        private string _RawValue;

        public FlString(string value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => StringType.Value;

        public override object RawValue => _RawValue;

        public string Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlString(_RawValue);
        }
    }
}
