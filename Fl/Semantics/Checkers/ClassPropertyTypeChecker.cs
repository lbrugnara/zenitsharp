using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ClassPropertyTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassPropertyNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassPropertyNode node)
        {
            // Get the property symbol
            var property = checker.SymbolTable.Get(node.Name.Value);

            // If the right-hand side is present, get the type
            var rhs = node.Definition?.Visit(checker);
            
            if (rhs != null && !property.TypeInfo.Type.IsAssignableFrom(rhs.TypeInfo.Type))
                throw new SymbolException($"Cannot assign type {rhs.TypeInfo} to variable of type {property.TypeInfo}");

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple property declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(property.TypeInfo, property);
        }
    }
}
