// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ObjectSymbol : SymbolContainer
    {
        protected List<string> Properties { get; set; }
        protected List<string> Functions { get; set; }
        
        public ObjectSymbol(string name, SymbolContainer parent = null)
            : base(name, parent)
        {
            this.Properties = new List<string>();
            this.Functions = new List<string>();
        }

        public Symbol CreateProperty(string name, TypeInfo type, Access access, Storage storage)
        {
            var symbol = this.CreateSymbol(name, type, access, storage);
            this.Properties.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateFunction(string name, TypeInfo type, Access access)
        {
            var symbol = this.CreateSymbol(name, type, access, Storage.Constant);
            this.Functions.Add(symbol.Name);
            return symbol;
        }
    }
}
