// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AccessorNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, AccessorNode accessor)
        {
            // The target scope is the current scope, unless the accessor node has a parent
            var owner = accessor.Parent?.Visit(visitor) as ISymbolContainer ?? visitor.SymbolTable.CurrentScope;

            return owner.TryGet<IBoundSymbol>(accessor.Target.Value)?.TypeSymbol;
        }
    }
}
