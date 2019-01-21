using Fl.Ast;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Resolvers
{
    class ClassConstantSymbolResolver : INodeVisitor<SymbolResolverVisitor, ClassConstantNode, ISymbol>
    {
        public ISymbol Visit(SymbolResolverVisitor binder, ClassConstantNode node)
        {
            /*var classScope = binder.SymbolTable.CurrentScope as ClassScope;

            if (classScope == null)
                throw new SymbolException($"Current scope is not a class scope ({binder.SymbolTable.CurrentScope.GetType().Name})");

            // Get the constant name
            var constantName = node.Name.Value;

            // Check if the symbol is already defined
            if (classScope.HasSymbol(constantName))
                throw new SymbolException($"Symbol {constantName} is already defined.");

            // Get the constant type from the declaration or assume an anonymous type
            var lhsType = SymbolHelper.GetType(binder.SymbolTable, binder.Inferrer, node.SymbolInfo.Type);

            // Create the new symbol for the property
            var symbol = classScope.CreateConstant(constantName, lhsType, SymbolHelper.GetAccess(node.SymbolInfo.Access));

            // If it is a type assumption, register the symbol under that assumption
            if (binder.Inferrer.IsTypeAssumption(lhsType))
                binder.Inferrer.AddTypeDependency(lhsType, symbol);

            // Visit the right-hand side expression
            node.Definition.Visit(binder);*/

            return null;
        }
    }
}
