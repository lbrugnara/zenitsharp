// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;

namespace Fl.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AccessorNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, AccessorNode accessor)
        {
            // The target scope is the current scope, unless the accessor node has a parent
            var owner = accessor.Parent?.Visit(visitor) as IContainer 
                        ?? visitor.SymbolTable.CurrentScope;

            // If the symbol is not defined in the current scope, we create an unresolved type symbol for it
            return owner.TryGet<ISymbol>(accessor.Target.Value) 
                    ?? new UnresolvedTypeSymbol(accessor.Target.Value, owner);
        }
    }
}
