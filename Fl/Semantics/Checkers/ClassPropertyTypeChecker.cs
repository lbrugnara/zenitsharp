using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class ClassPropertyTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassPropertyNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassPropertyNode node)
        {
            // Get the property symbol
            var property = checker.SymbolTable.GetBoundSymbol(node.Name.Value);

            // If the right-hand side is present, get the type
            var rhs = node.Definition?.Visit(checker);
            
            /*if (rhs != null && !property.TypeSymbol.Type.IsAssignableFrom(rhs.TypeSymbol.Type))
                throw new SymbolException($"Cannot assign type {rhs.TypeSymbol} to variable of type {property.TypeSymbol}");*/

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple property declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(property.TypeSymbol, property);
        }
    }
}
