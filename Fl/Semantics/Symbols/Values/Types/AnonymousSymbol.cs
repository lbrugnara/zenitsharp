// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;

namespace Fl.Semantics.Symbols
{
    public class AnonymousSymbol : ITypeSymbol
    {
        public string Name { get; }

        public ISymbolContainer Parent { get; }

        public BuiltinType BuiltinType => BuiltinType.Anonymous;

        public AnonymousSymbol(string name)
        {
            this.Name = name;
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
