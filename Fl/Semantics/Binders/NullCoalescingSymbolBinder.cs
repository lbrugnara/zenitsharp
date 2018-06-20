// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Binders
{
    class NullCoalescingSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstNullCoalescingNode>
    {
        public void Visit(SymbolBinderVisitor visitor, AstNullCoalescingNode nullc)
        {
            nullc.Left.Visit(visitor);
            nullc.Right.Visit(visitor);
        }
    }
}
