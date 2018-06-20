// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class BinarySymbolBinder : INodeVisitor<SymbolBinderVisitor, AstBinaryNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstBinaryNode binary)
        {
            binary.Left.Visit(visitor);
            binary.Right.Visit(visitor);
        }
    }
}
