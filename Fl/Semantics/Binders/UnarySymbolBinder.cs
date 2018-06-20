// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class UnarySymbolBinder : INodeVisitor<SymbolBinderVisitor, AstUnaryNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstUnaryNode unary)
        {
            unary.Left.Visit(visitor);
        }
    }
}
