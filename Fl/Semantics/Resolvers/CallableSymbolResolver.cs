// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, CallableNode>
    {
        public void Visit(SymbolResolverVisitor visitor, CallableNode node)
        {
            node.Target.Visit(visitor);
            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));
        }
    }
}
