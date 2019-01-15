// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolverVisitor, UnaryNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, UnaryNode unary)
        {
            return unary.Left.Visit(visitor);
        }
    }
}
