// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, BreakNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, BreakNode wnode)
        {
            wnode.Number?.Visit(visitor);

            return null;
        }
    }
}
