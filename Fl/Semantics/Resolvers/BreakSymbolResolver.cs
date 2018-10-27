// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, BreakNode>
    {
        public void Visit(SymbolResolverVisitor visitor, BreakNode wnode)
        {
            wnode.Number?.Visit(visitor);
        }
    }
}
