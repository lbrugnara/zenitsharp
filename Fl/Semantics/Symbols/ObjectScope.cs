// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ObjectScope : Scope
    {
        protected List<string> Properties { get; set; }
        protected List<string> Functions { get; set; }
        
        public ObjectScope(string uid, Scope parent = null)
            : base(uid, parent)
        {
            this.Properties = new List<string>();
            this.Functions = new List<string>();
        }

        public override ScopeType Type => ScopeType.Object;

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
