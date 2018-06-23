using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassConstantNode node)
        {
            // Get the constant symbol
            var constant = checker.SymbolTable.GetSymbol(node.Name.Value.ToString());

            // Use the ClassProperty.Type property in the type checking
            var lhsType = (constant.Type as ClassProperty).Type;

            // Get the right-hand side expression's type
            var rhs = node.Definition.Visit(checker);            

            if (!lhsType.IsAssignableFrom(rhs.Type))
                throw new SymbolException($"Cannot assign type {rhs.Type} to constant of type {lhsType}");

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple constant declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(constant.Type, constant);            
        }
    }
}
