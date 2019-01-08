// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class FunctionSymbol : ComplexSymbol
    {
        public List<string> Parameters { get; set; }

        public FunctionSymbol(string name, ISymbolContainer parent = null)
            : base (name, parent)
        {
            this.Parameters = new List<string>();
            this.ReturnSymbol = new Symbol(
                "@ret",
                null,
                Access.Public,
                Storage.Mutable, 
                this
            );
            this.Add(this.ReturnSymbol);
        }

        public Symbol ReturnSymbol { get; private set; }

        public void UpdateReturnType(TypeInfo typeInfo)
        {
            this.ReturnSymbol.TypeInfo = typeInfo ?? throw new System.ArgumentNullException(nameof(typeInfo), "Return type cannot be null");

            var self = this.Parent.Get<FunctionSymbol>(this.Name);

            (self?.TypeInfo.Type as Function)?.SetReturnType(typeInfo.Type);
        }

        public Symbol CreateParameter(string name, TypeInfo typeInfo, Access access, Storage storage)
        {
            var symbol = new Symbol(name, typeInfo, access, storage, this);
            this.Add(symbol);
            this.Parameters.Add(symbol.Name);
            return symbol;
        }
    }
}
