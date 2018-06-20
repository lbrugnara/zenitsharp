// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class BreakSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstBreakNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstBreakNode wnode)
        {
            wnode.Number?.Visit(visitor);
        }
    }
}
