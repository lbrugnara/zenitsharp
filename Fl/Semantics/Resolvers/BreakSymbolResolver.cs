// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class BreakSymbolResolver : INodeVisitor<SymbolResolverVisitor, BreakNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, BreakNode wnode)
        {
            wnode.Number?.Visit(visitor);

            return null;
        }
    }
}
