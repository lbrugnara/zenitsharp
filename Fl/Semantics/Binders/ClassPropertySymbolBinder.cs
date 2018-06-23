using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
using System;
using System.Collections.Generic;

namespace Fl.Semantics.Binders
{
    class ClassPropertySymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassPropertyNode>
    {
        public void Visit(SymbolBinderVisitor binder, AstClassPropertyNode node)
        {
            // Get the property name
            var propertyName = node.Name.Value.ToString();

            // Check if the symbol is already defined
            if (binder.SymbolTable.HasSymbol(propertyName))
                throw new SymbolException($"Symbol {propertyName} is already defined.");

            // Get the property type, access modifier, and storage type
            var type = SymbolHelper.GetType(binder.SymbolTable, node.Type.Name) ?? binder.Inferrer.NewAnonymousType();
            var accessMod = SymbolHelper.GetAccessModifier(node.AccessModifier);
            var storageType = SymbolHelper.GetStorageType(node.StorageType);

            // Create the property type
            var propertyType = new ClassProperty(type, accessMod, storageType);

            // Create the new symbol for the property
            var symbol = binder.SymbolTable.NewSymbol(propertyName, propertyType);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(propertyType))
                binder.Inferrer.AssumeSymbolTypeAs(symbol, propertyType);

            // If the property has a definition, visit the right-hand side expression
            node.Definition?.Visit(binder);
        }
    }
}
