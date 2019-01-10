// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, BreakNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, BreakNode wnode)
        {
            wnode.Number?.Visit(visitor);

            return null;
        }
    }
}
