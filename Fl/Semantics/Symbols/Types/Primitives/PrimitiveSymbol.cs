// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class PrimitiveSymbol : TypeSymbol, IPrimitiveSymbol
    {
        public PrimitiveSymbol(BuiltinType type, IContainer parent)
            : base(type, parent)
        {
        }

        public override string ToString()
        {
            return this.BuiltinType.GetName();
        }

        public override string ToValueString() => this.ToString();
    }
}
