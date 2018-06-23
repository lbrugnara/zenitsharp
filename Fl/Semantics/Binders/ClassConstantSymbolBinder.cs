using Fl.Ast;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;
using System;

namespace Fl.Semantics.Binders
{
    class ClassConstantSymbolBinder : INodeVisitor<SymbolBinderVisitor, AstClassConstantNode>
    {
        public void Visit(SymbolBinderVisitor binder, AstClassConstantNode node)
        {
            // Get the constant name
            var constantName = node.Name.Value.ToString();

            // Check if the symbol is already defined
            if (binder.SymbolTable.HasSymbol(constantName))
                throw new SymbolException($"Symbol {constantName} is already defined.");

            // Get the constant type from the declaration or assume an anonymous type
            var type = SymbolHelper.GetType(binder.SymbolTable, node.Type) ?? binder.Inferrer.NewAnonymousType();
            var lhsType = new ClassProperty(type, SymbolHelper.GetAccessModifier(node.AccessModifier), StorageType.Const);

            // Create the new symbol for the property
            var symbol = binder.SymbolTable.NewSymbol(constantName, lhsType);

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(lhsType))
                binder.Inferrer.AssumeSymbolTypeAs(symbol, lhsType);

            // Visit the right-hand side expression
            node.Definition.Visit(binder);
        }
    }
}
