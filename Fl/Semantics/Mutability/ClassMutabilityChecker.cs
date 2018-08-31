using Fl.Ast;
using Fl.Semantics.Checkers;

namespace Fl.Semantics.Mutability
{
    class ClassMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, AstClassNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, AstClassNode node)
        {
            checker.SymbolTable.EnterClassScope(node.Name.Value.ToString());

            node.Properties.ForEach(propertyNode => propertyNode.Visit(checker));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(checker));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new MutabilityCheckResult(checker.SymbolTable.GetSymbol(node.Name.Value.ToString()));
        }
    }
}
