// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    public class MutabilityCheckResult
    {
        public IBoundSymbol Symbol { get; set; }

        public MutabilityCheckResult(IBoundSymbol symbol)
        {
            this.Symbol = symbol;
        }
    }
}
