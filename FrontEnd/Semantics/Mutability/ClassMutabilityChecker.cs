using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class ClassMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, ClassNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, ClassNode node)
        {
            checker.SymbolTable.EnterClassScope(node.Name.Value);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(checker));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(checker));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new MutabilityCheckResult(checker.SymbolTable.GetVariableSymbol(node.Name.Value));
        }
    }
}
