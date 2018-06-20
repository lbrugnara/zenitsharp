// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class AccessorSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstAccessorNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstAccessorNode accessor)
        {
            accessor.Enclosing?.Visit(visitor);
        }
    }
}
