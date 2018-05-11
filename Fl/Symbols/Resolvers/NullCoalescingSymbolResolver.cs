// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class NullCoalescingSymbolResolver : INodeVisitor<SymbolResolver, AstNullCoalescingNode>
    {
        public void Visit(SymbolResolver checker, AstNullCoalescingNode nullc)
        {
            nullc.Left.Visit(checker);
            nullc.Right.Visit(checker);
        }
    }
}
