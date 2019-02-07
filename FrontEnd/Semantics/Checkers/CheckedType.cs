// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Semantics.Symbols.Types;
using Zenit.Semantics.Symbols.Values;

namespace Zenit.Semantics.Checkers
{
    public class CheckedType
    {
        public ITypeSymbol TypeSymbol { get; set; }
        public IBoundSymbol Symbol { get; set; }

        public CheckedType(ITypeSymbol type, IBoundSymbol symbol = null)
        {
            this.TypeSymbol = type;
            this.Symbol = symbol;
        }
    }
}
