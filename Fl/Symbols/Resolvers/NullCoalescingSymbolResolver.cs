// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class NullCoalescingSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstNullCoalescingNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstNullCoalescingNode nullc)
        {
            nullc.Left.Visit(visitor);
            nullc.Right.Visit(visitor);
        }
    }
}
