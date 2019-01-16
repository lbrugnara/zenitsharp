// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Symbols.Values;
using Fl.Semantics.Types;

namespace Fl.Semantics.Resolvers
{
    class ObjectPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ObjectPropertyNode, IValueSymbol>
    {
        public IValueSymbol Visit(SymbolResolverVisitor visitor, ObjectPropertyNode node)
        {
            var storage = SymbolHelper.GetStorage(node.Information.Mutability);

            // Visit the property's value node
            var rhsSymbol = visitor.Visit(node.Value);

            if (rhsSymbol != null)
            {
                visitor.SymbolTable.Insert(node.Name.Value, new BoundSymbol(node.Name.Value, rhsSymbol.GetTypeSymbol(), Access.Public, storage, visitor.SymbolTable.CurrentScope));
                return rhsSymbol;
            }

            var typeSymbol = SymbolHelper.GetTypeSymbol(visitor.SymbolTable, visitor.Inferrer, node.Information.Type);

            // Create the symbol for the object's property
            visitor.SymbolTable.Insert(node.Name.Value, typeSymbol, Access.Public, storage);

            return typeSymbol;
        }
    }
}
