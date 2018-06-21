using Fl.Ast;
using Fl.Semantics.Checkers;

namespace Fl.Semantics.Inferrers
{
    class ClassPropertyTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassPropertyNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassPropertyNode node)
        {
            // TODO: Implement ClassProperty Checker
            return null;
        }
    }
}
