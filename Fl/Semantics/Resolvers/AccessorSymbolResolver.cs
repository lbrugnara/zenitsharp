// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstAccessorNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AstAccessorNode accessor)
        {
            accessor.Enclosing?.Visit(visitor);
        }
    }
}
