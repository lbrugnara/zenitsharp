// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolverVisitor, UnaryNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, UnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
