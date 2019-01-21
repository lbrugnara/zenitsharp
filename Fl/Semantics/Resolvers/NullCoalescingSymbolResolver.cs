// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class NullCoalescingSymbolResolver : INodeVisitor<SymbolResolverVisitor, NullCoalescingNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(left.GetTypeSymbol(), right.GetTypeSymbol());
        }
    }
}
