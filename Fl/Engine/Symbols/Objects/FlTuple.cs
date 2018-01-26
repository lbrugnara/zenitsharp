// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Types;
using System.Collections.Generic;
using System.Linq;

namespace Fl.Engine.Symbols.Objects
{
    public class FlTuple : FlInstance
    {
        private List<FlObject> _RawValue;

        public FlTuple(List<FlObject> value = null)
        {
            _RawValue = value ?? new List<FlObject>();
        }

        public override FlType Type => FlTupleType.Instance;

        public override object RawValue => _RawValue;

        public List<FlObject> Value { get => _RawValue; set => _RawValue = value; }

        public override FlObject Clone()
        {
            return new FlTuple(_RawValue);
        }
        
        public override string ToString()
        {
            return $"({string.Join(", ", _RawValue.Select(o => o.ToString()))}) ({Type})";
        }

        public override string ToDebugStr()
        {
            return $"({string.Join(", ", _RawValue.Select(o => o.ToDebugStr()))}) ({Type})";
        }
    }
}
