// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class NoneSymbol : ITypeSymbol
    {
        public string Name => "none";

        public ISymbolContainer Parent => null;

        public BuiltinType BuiltinType => BuiltinType.None;

        public NoneSymbol()
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
