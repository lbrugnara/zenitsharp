// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Exceptions;
using Fl.Engine.Symbols.Types;

namespace Fl.Engine.Symbols.Objects
{
    public class FlBoolean : FlObject
    {
        private bool _RawValue;

        public FlBoolean(bool value)
        {
            _RawValue = value;
        }

        public override ObjectType ObjectType => BoolType.Value;

        public override object RawValue => _RawValue;

        public bool Value { get => _RawValue; set => _RawValue = value; }

        public override bool IsPrimitive => true;

        public override FlObject Clone()
        {
            return new FlBoolean(_RawValue);
        }

        public override FlObject ConvertTo(ObjectType type)
        {
            if (type == StringType.Value)
            {
                return new FlString(_RawValue.ToString());
            }
            throw new CastException($"Cannot convert type {ObjectType} to {type}");
        }
    }
}
