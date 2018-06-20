// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class ContinueSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstContinueNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstContinueNode cnode)
        {
        }
    }
}
