// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class VoidSymbol : ITypeSymbol
    {
        public string Name => "void";

        public ISymbolContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.Void;        

        public VoidSymbol()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToValueString()
        {
            return this.Name;
        }
    }
}
