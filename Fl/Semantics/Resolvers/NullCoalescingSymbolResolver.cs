// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class NullCoalescingSymbolResolver : INodeVisitor<SymbolResolverVisitor, NullCoalescingNode>
    {
        public void Visit(SymbolResolverVisitor visitor, NullCoalescingNode nullc)
        {
            nullc.Left.Visit(visitor);
            nullc.Right.Visit(visitor);
        }
    }
}
