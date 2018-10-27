using Fl.Ast;
using Fl.Semantics.Checkers;
using Fl.Semantics.Exceptions;
using Fl.Semantics.Types;

namespace Fl.Semantics.Mutability
{
    class ClassPropertyMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ClassPropertyNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ClassPropertyNode node)
        {
            node.Definition?.Visit(checker);
            return null;
        }
    }
}
