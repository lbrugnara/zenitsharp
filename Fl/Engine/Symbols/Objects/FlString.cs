// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
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

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == StringType.Value)
            {
                return this.Clone();
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }

        public override string ToDebugStr()
        {
            return $"\"{RawValue}\" ({ObjectType})";
        }
    }
}
