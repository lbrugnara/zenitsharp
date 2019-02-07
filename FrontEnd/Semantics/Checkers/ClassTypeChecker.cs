using Zenit.Ast;

namespace Zenit.Semantics.Checkers
{
    class ClassTypeChecker : INodeVisitor<TypeCheckerVisitor, ClassNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ClassNode node)
        {
            checker.SymbolTable.EnterClassScope(node.Name.Value);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(checker));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(checker));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(checker.SymbolTable.GetBoundSymbol(node.Name.Value).TypeSymbol);
        }
    }
}
