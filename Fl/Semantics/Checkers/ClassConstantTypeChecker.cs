using Fl.Ast;
using Fl.Semantics.Checkers;

namespace Fl.Semantics.Inferrers
{
    class ClassConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassConstantNode node)
        {
            // TODO: Implement ClassConstant Checker
            return null;
        }
    }
}
