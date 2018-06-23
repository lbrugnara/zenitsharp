using Fl.Ast;
using Fl.Semantics.Checkers;

namespace Fl.Semantics.Inferrers
{
    class ClassTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassNode node)
        {
            checker.SymbolTable.EnterClassScope(node.Name.Value.ToString());

            node.Properties.ForEach(propertyNode => propertyNode.Visit(checker));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(checker));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(checker));

            checker.SymbolTable.LeaveScope();

            return new CheckedType(checker.SymbolTable.Global.GetSymbol(node.Name.Value.ToString()).Type);
        }
    }
}
