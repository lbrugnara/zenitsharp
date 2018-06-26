// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstCallableNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstCallableNode node)
        {
            node.Callable.Visit(visitor);
            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));
        }
    }
}
