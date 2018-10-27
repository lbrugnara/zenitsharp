// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AccessorNode>
    {
        public void Visit(SymbolResolverVisitor visitor, AccessorNode accessor)
        {
            accessor.Parent?.Visit(visitor);
        }
    }
}
