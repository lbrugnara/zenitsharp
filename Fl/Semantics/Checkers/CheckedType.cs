// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    public class CheckedType
    {
        public Struct Type { get; set; }
        public Symbol Symbol { get; set; }

        public CheckedType(Struct type, Symbol symbol = null)
        {
            this.Type = type;
            this.Symbol = symbol;
        }
    }
}
