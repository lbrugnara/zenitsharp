// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class ContinueSymbolResolver : INodeVisitor<SymbolResolverVisitor, ContinueNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, ContinueNode cnode)
        {
            return null;
        }
    }
}
