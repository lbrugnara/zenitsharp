using Zenit.Ast;

namespace Zenit.Semantics.Mutability
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
