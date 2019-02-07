// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;

namespace Zenit.Semantics.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolverVisitor, ContinueNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ContinueNode cnode)
        {
            return null;
        }
    }
}
