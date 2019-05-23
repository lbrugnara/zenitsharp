// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Mutability
{
    public class MutabilityCheckResult
    {
        public IVariable Symbol { get; set; }

        public MutabilityCheckResult(IVariable symbol)
        {
            this.Symbol = symbol;
        }
    }
}
