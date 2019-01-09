// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ObjectSymbol : ComplexSymbol
    {
        protected List<string> Properties { get; set; }
        protected List<string> Functions { get; set; }
        
        public ObjectSymbol(string name, ISymbolContainer parent = null)
            : base(name, new TypeInfo(new Object(BuiltinType.Object)), Access.Public, Storage.Constant, parent)
        {
            this.Properties = new List<string>();
            this.Functions = new List<string>();
        }

        public Symbol CreateProperty(string name, TypeInfo type, Access access, Storage storage)
        {
            var symbol = new Symbol(name, type, access, storage, this);
            this.Insert(symbol);
            this.Properties.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateFunction(string name, TypeInfo type, Access access)
        {
            var symbol = new FunctionSymbol(name, this);
            this.Insert(symbol);
            this.Functions.Add(symbol.Name);
            return symbol;
        }
    }
}
