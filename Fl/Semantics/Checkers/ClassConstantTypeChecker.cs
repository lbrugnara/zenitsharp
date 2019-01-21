using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class ClassConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassConstantNode node)
        {
            // Get the constant symbol
            var constant = checker.SymbolTable.GetBoundSymbol(node.Name.Value);

            // Get the right-hand side expression's type
            var rhs = node.Definition.Visit(checker);            

            /*if (!constant.TypeSymbol.IsAssignableFrom(rhs.TypeSymbol.Type))
                throw new SymbolException($"Cannot assign type {rhs.TypeSymbol} to constant of type {constant.TypeSymbol}");*/

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple constant declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(constant.TypeSymbol, constant);            
        }
    }
}
