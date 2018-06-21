using Fl.Ast;
using Fl.Semantics.Checkers;

namespace Fl.Semantics.Inferrers
{
    class ClassTypeChecker : INodeVisitor<TypeCheckerVisitor, AstClassNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstClassNode node)
        {
            // TODO: Implement Class Checker
            return new CheckedType(checker.SymbolTable.Global.GetSymbol(node.Name.Value.ToString()).Type);
        }
    }
}
