// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    public class InferredType
    {
        public Type Type { get; set; }
        public Symbol Symbol { get; set; }

        public InferredType(Type type, Symbol symbol = null)
        {
            this.Type = type;
            this.Symbol = symbol;
        }
    }
}
