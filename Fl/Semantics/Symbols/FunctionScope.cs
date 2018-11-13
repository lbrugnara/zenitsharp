// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Types;
using System.Collections.Generic;

namespace Fl.Semantics.Symbols
{
    public class FunctionScope : Scope
    {
        protected List<string> Parameters { get; set; }

        public FunctionScope(string uid, Scope parent = null)
            : base (uid, parent)
        {
            this.ReturnSymbol = this.CreateSymbol("@ret", null, Access.Public, Storage.Mutable);
            this.Parameters = new List<string>();
        }

        public override ScopeType Type => ScopeType.Function;

        public Symbol ReturnSymbol { get; }

        public Symbol CreateParameter(string name, Type type, Access access, Storage storage)
        {
            var symbol = this.CreateSymbol(name, type, access, storage);
            this.Parameters.Add(symbol.Name);
            return symbol;
        }
    }
}
