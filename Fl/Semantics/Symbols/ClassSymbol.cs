// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ClassSymbol : ComplexSymbol
    {
        protected List<string> Properties { get; set; }
        protected List<string> Constants { get; set; }
        protected List<string> Methods { get; set; }

        public ClassSymbol(string name, ISymbolContainer parent = null)
            : base(name, parent)
        {
            this.Properties = new List<string>();
            this.Constants = new List<string>();
            this.Methods = new List<string>();
        }

        public Symbol CreateProperty(string name, TypeInfo type, Access access, Storage storage)
        {
            var symbol = new Symbol(name, type, access, storage, this);
            this.Add(symbol);
            this.Properties.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateConstant(string name, TypeInfo type, Access access)
        {
            var symbol = new Symbol(name, type, access, Storage.Constant, this);
            this.Add(symbol);
            this.Constants.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateMethod(string name, TypeInfo type, Access access)
        {
            var symbol = new FunctionSymbol(name, this);
            this.Add(symbol);
            this.Methods.Add(symbol.Name);
            return symbol;
        }
    }
}
