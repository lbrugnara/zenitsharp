using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class ClassConstantMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ClassConstantNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ClassConstantNode node)
        {
            node.Definition.Visit(checker);

            return null;
        }
    }
}
