// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols.Containers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Types.Specials;
using Fl.Semantics.Symbols.Values;

namespace Fl.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AccessorNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, AccessorNode accessor)
        {
            // If there is a parent, it must be an IContainer
            IContainer owner = null;

            var ownerSymbol = accessor.Parent?.Visit(visitor);

            // If the ownerSymbol is present, process it to get the IContainer
            if (ownerSymbol != null)
            {
                if (ownerSymbol is IBoundSymbol bs)
                    owner = (IContainer)bs.GetTypeSymbol();
                else
                    owner = (IContainer)ownerSymbol;
            }
            else
            {
                // If the owner symbol is null, the current scope is the owner
                owner = visitor.SymbolTable.CurrentScope;
            }
            
            return  // 1- Try to get a bound variable
                    owner.TryGet<IBoundSymbol>(accessor.Target.Value) 
                    
                    // 2- Try to get a package
                    ?? owner.TryGet<IPackage>(accessor.Target.Value)
                    
                    // 3- Return an unresolved type symbo
                    ?? (ISymbol)new UnresolvedTypeSymbol(accessor.Target.Value, owner);
        }
    }
}
