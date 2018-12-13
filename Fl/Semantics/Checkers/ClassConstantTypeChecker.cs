using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ClassConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassConstantNode node)
        {
            // Get the constant symbol
            var constant = checker.SymbolTable.GetSymbol(node.Name.Value);

            // Get the right-hand side expression's type
            var rhs = node.Definition.Visit(checker);            

            if (!constant.TypeInfo.Type.IsAssignableFrom(rhs.TypeInfo.Type))
                throw new SymbolException($"Cannot assign type {rhs.TypeInfo} to constant of type {constant.TypeInfo}");

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple constant declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new CheckedType(constant.TypeInfo, constant);            
        }
    }
}
