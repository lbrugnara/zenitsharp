// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols.Types;
using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class PrimitiveSymbol : IPrimitiveSymbol
    {
        public BuiltinType BuiltinType { get; }

        public IContainer Parent { get; }

        public PrimitiveSymbol(BuiltinType type, IContainer parent)
        {
            this.BuiltinType = type;
            this.Parent = parent;
        }

        public string Name => this.BuiltinType.GetName();

        public override string ToString()
        {
            return this.BuiltinType.GetName();
        }

        public virtual string ToValueString() => this.ToString();
    }
}
