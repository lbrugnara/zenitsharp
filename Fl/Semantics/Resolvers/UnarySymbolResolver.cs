// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class UnarySymbolResolver : INodeVisitor<SymbolResolverVisitor, UnaryNode>
    {
        public void Visit(SymbolResolverVisitor visitor, UnaryNode unary)
        {
            unary.Left.Visit(visitor);
        }
    }
}
