// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ObjectSymbol : ComplexSymbol
    {
        public ObjectSymbol(string name, ISymbolContainer parent)
            : base(name, BuiltinType.Object, parent)
        {
        }

        public IBoundSymbol CreateProperty(string name, ITypeSymbol type, Access access, Storage storage)
        {
            var symbol = new BoundSymbol(name, type, access, storage, this);

            this.Insert(name, symbol);
            this.Properties[name] = symbol;

            return symbol;
        }

        public IBoundSymbol CreateFunction(string name, ITypeSymbol type, Access access)
        {
            var symbol = new BoundSymbol(name, type, access, Storage.Constant, this);

            this.Insert(name, symbol);
            this.Functions[name] = symbol;

            return symbol;
        }
    }
}
