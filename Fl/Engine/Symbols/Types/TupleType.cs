// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.Symbols.Objects;
using System.Collections.Generic;

namespace Fl.Engine.Symbols.Types
{
    public class TupleType : ObjectType
    {
        private static TupleType _Instance;

        private TupleType() { }

        public static TupleType Value => _Instance != null ? _Instance : (_Instance = new TupleType());

        public override string Name => "tuple";

        public override string ClassName => "tuple";

        public override object RawDefaultValue()
        {
            throw new System.NotImplementedException();
        }

        public override FlObject DefaultValue()
        {
            return new FlTuple();
        }

        public override FlObject NewValue(object o)
        {
            return new FlTuple(o as List<FlObject>);
        }
    }
}
