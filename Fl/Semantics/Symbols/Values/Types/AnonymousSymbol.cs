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

        public AnonymousSymbol(string name = "'a")
        {
            this.Name = name;
        }

        public string ToDebugString(int indent = 0)
        {
            return this.Name;
        }
    }
}
