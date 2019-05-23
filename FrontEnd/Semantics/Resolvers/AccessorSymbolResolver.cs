// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Containers;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols.Types.Specials;
using Zenit.Semantics.Symbols.Variables;
using Zenit.Semantics.Symbols.Types.Specials.Unresolved;

namespace Zenit.Semantics.Resolvers
{
    class AccessorSymbolResolver : INodeVisitor<SymbolResolverVisitor, AccessorNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, AccessorNode accessor)
        {
            // If there is a parent, it must be an IContainer
            IContainer parent = null;

            if (accessor.Parent != null)
            {
                var parentSymbol = accessor.Parent.Visit(visitor);
                // A variable symbol can only be used as an IContainer if its underlying type is an IContainer
                parent = parentSymbol is IVariable bs ? (IContainer)bs.GetTypeSymbol() : (IContainer)parentSymbol;
            }
            else
            {
                parent = visitor.SymbolTable.CurrentScope;
            }

            // 1- Try to get the target symbol as a variable symbol
            // 2- Try to get it as a package
            // 3- Create an unresolved type symbol
            return parent.TryGet<IVariable>(accessor.Target.Value) 
                   ?? parent.TryGet<IPackage>(accessor.Target.Value)
                   ?? (ISymbol)new UnresolvedSymbol(accessor.Target.Value, parent);
        }
    }
}
