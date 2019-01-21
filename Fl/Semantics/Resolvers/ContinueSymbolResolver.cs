// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolverVisitor, ContinueNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ContinueNode cnode)
        {
            return null;
        }
    }
}
