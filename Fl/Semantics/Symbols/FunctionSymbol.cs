// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class FunctionSymbol : SymbolContainer
    {
        public List<string> Parameters { get; set; }

        public FunctionSymbol(string name, SymbolContainer parent = null)
            : base (name, parent)
        {
            this.Parameters = new List<string>();
            this.ReturnSymbol = this.CreateSymbol(
                "@ret",
                null,
                Access.Public,
                Storage.Mutable
            );
        }

        public Symbol ReturnSymbol { get; private set; }

        public void UpdateReturnType(TypeInfo typeInfo)
        {
            this.ReturnSymbol.TypeInfo = typeInfo ?? throw new System.ArgumentNullException(nameof(typeInfo), "Return type cannot be null");
            (this.Parent.GetSymbol(this.Name).TypeInfo.Type as Function)
                .SetReturnType(typeInfo.Type);
        }

        public Symbol CreateParameter(string name, TypeInfo typeInfo, Access access, Storage storage)
        {
            var symbol = this.CreateSymbol(name, typeInfo, access, storage);
            this.Parameters.Add(symbol.Name);
            return symbol;
        }
    }
}
