// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class TupleSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstTupleNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstTupleNode node)
        {
            node.Items?.ForEach(item => item.Visit(visitor));
        }
    }
}
