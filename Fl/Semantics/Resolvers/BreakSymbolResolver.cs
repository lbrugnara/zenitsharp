// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstBreakNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstBreakNode wnode)
        {
            wnode.Number?.Visit(visitor);
        }
    }
}
