using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class ClassConstantMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstClassConstantNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstClassConstantNode node)
        {
            node.Definition.Visit(checker);

            return null;
        }
    }
}
