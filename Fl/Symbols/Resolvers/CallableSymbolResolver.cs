// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    public class CallableSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstCallableNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstCallableNode node)
        {
            node.Callable.Visit(checker);
            node.Arguments.Expressions.ForEach(e => e.Visit(checker));
        }
    }
}
