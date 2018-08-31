using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ClassPropertyTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassPropertyNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassPropertyNode node)
        {
            // Get the property symbol
            var property = checker.SymbolTable.GetSymbol(node.Name.Value.ToString());

            // If the right-hand side is present, get the type
            var rhs = node.Definition?.Visit(checker);
            
            if (rhs != null && !property.Type.IsAssignableFrom(rhs.Type))
                throw new SymbolException($"Cannot assign type {rhs.Type} to variable of type {property.Type}");

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple property declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(property.Type, property);
        }
    }
}
