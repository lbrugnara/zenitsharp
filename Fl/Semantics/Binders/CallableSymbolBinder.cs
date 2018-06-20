// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    public class CallableSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstCallableNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstCallableNode node)
        {
            node.Callable.Visit(visitor);
            node.Arguments.Expressions.ForEach(e => e.Visit(visitor));
        }
    }
}
