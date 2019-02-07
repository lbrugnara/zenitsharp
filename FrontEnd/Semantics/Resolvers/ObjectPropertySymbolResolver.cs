// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Resolvers
{
    class ObjectPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectPropertyNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor visitor, ObjectPropertyNode node)
        {
            var storage = SymbolHelper.GetStorage(node.Information.Mutability);

            // Visit the property's value node
            var rhsSymbol = visitor.Visit(node.Value);

            if (rhsSymbol != null)
            {
                visitor.SymbolTable.BindSymbol(node.Name.Value, rhsSymbol.GetTypeSymbol(), Access.Public, storage);
                return rhsSymbol;
            }

            var typeSymbol = SymbolHelper.GetTypeSymbol(visitor.SymbolTable, visitor.Inferrer, node.Information.Type);

            // Create the symbol for the object's property
            visitor.SymbolTable.BindSymbol(node.Name.Value, typeSymbol, Access.Public, storage);

            return typeSymbol;
        }
    }
}
