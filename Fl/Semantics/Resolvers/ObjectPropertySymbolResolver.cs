// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ObjectPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectPropertyNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor visitor, ObjectPropertyNode node)
        {
            ITypeSymbol typeSymbol = null;

            // Visit the property's value node
            var rhsSymbol = visitor.Visit(node.Value);

            if (rhsSymbol == null)
            {
                // Create the symbol for the object's property
                var boundSymbol = visitor.SymbolTable.Insert(
                    node.Name.Value,
                    SymbolHelper.GetTypeSymbol(visitor.SymbolTable, visitor.Inferrer, node.Information.Type),
                    Symbols.Access.Public,
                    SymbolHelper.GetStorage(node.Information.Mutability)
                );
                typeSymbol = boundSymbol.TypeSymbol;
            }
            else
            {
                typeSymbol = rhsSymbol;
                visitor.SymbolTable.Insert(node.Name.Value, new BoundSymbol(node.Name.Value, rhsSymbol, Access.Public, SymbolHelper.GetStorage(node.Information.Mutability), visitor.SymbolTable.CurrentScope));
            }

            return typeSymbol;
        }
    }
}
