// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlString : FlInstance
    {
        private string _RawValue;

        public FlString(string value)
        {
            _RawValue = value;
        }

        public override FlType Type => FlStringType.Instance;

        public override object RawValue => _RawValue;

        public string Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlString(_RawValue);
        }

        public override string ToDebugStr()
        {
            return $"\"{RawValue}\" ({Type})";
        }
    }
}
