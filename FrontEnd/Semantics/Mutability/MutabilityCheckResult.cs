// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Values;

namespace Zenit.Semantics.Mutability
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
