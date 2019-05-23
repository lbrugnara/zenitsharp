using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class ClassTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassNode, IType>
    {
        public IType Visit(TypeInferrerVisitor inferrer, ClassNode node)
        {
            inferrer.SymbolTable.EnterClassScope(node.Name.Value);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(inferrer));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(inferrer));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(inferrer));

            inferrer.SymbolTable.LeaveScope();

            return inferrer.SymbolTable.GetVariableSymbol(node.Name.Value).TypeSymbol;
        }
    }
}
