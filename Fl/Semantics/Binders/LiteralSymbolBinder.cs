// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class LiteralSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstLiteralNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstLiteralNode literal)
        {
        }
    }
}
