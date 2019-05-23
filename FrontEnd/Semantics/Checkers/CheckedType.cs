// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Variables;

namespace Zenit.Semantics.Checkers
{
    public class CheckedType
    {
        public IType TypeSymbol { get; set; }
        public IVariable Symbol { get; set; }

        public CheckedType(IType type, IVariable symbol = null)
        {
            this.TypeSymbol = type;
            this.Symbol = symbol;
        }
    }
}
