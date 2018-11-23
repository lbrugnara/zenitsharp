// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    public class InferredType
    {
        public Struct Type { get; set; }
        public Symbol Symbol { get; set; }

        public InferredType(Struct type, Symbol symbol = null)
        {
            this.Type = type;
            this.Symbol = symbol;
        }
    }
}
