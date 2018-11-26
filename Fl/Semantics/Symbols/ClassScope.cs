// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class ClassScope : Scope
    {
        protected List<string> Properties { get; set; }
        protected List<string> Constants { get; set; }
        protected List<string> Methods { get; set; }

        public ClassScope(string uid, Scope parent = null)
            : base(uid, parent)
        {
            this.Properties = new List<string>();
            this.Constants = new List<string>();
            this.Methods = new List<string>();
        }

        public override ScopeType Type => ScopeType.Class;
        
        public Symbol CreateProperty(string name, Object type, Access access, Storage storage)
        {
            var symbol = this.CreateSymbol(name, type, access, storage);
            this.Properties.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateConstant(string name, Object type, Access access)
        {
            var symbol = this.CreateSymbol(name, type, access, Storage.Constant);
            this.Constants.Add(symbol.Name);
            return symbol;
        }

        public Symbol CreateMethod(string name, Object type, Access access)
        {
            var symbol = this.CreateSymbol(name, type, access, Storage.Constant);
            this.Methods.Add(symbol.Name);
            return symbol;
        }
    }
}
