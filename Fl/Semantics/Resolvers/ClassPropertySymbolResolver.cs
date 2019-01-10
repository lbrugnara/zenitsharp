using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;
using System.Collections.Generic;

namespace Fl.Semantics.Resolvers
{
    class ClassPropertySymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassPropertyNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(SymbolResolverVisitor binder, ClassPropertyNode node)
        {
            /*var classScope = binder.SymbolTable.CurrentScope as ClassScope;

            if (classScope == null)
                throw new SymbolException($"Current scope is not a class scope ({binder.SymbolTable.CurrentScope.GetType().Name})");

            // Get the property name
            var propertyName = node.Name.Value;

            // Check if the symbol is already defined
            if (classScope.HasSymbol(propertyName))
                throw new SymbolException($"Symbol {propertyName} is already defined.");

            // Create the property type
            var propertyType = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, node.SymbolInfo.Type);

            // Create the new symbol for the property
            var access = SymbolHelper.GetAccess(node.SymbolInfo.Access);
            var storage = SymbolHelper.GetStorage(node.SymbolInfo.Mutability);
            var symbol = classScope.CreateProperty(propertyName, propertyType, access, storage);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(propertyType))
                binder.Inferrer.AddTypeDependency(propertyType, symbol);

            // If the property has a definition, visit the right-hand side expression
            node.Definition?.Visit(binder);*/
            return null;
        }
    }
}
