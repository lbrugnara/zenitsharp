using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;
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

            // Get the property type from the declaration or assume an anonymous type
            var lhsType = TypeHelper.FromToken(node.Type.Name) ?? binder.Inferrer.NewAnonymousType();

            // Create the new symbol for the property
            var symbol = binder.SymbolTable.NewSymbol(propertyName, lhsType);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(lhsType))
                binder.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);

            // If the property has a definition, visit the right-hand side expression
            node.Definition?.Visit(binder);
        }
    }
}
