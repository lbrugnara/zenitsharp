// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    public class MutabilityCheckResult
    {
        public ISymbol Symbol { get; set; }

        public MutabilityCheckResult(ISymbol symbol)
        {
            this.Symbol = symbol;
        }
    }
}
