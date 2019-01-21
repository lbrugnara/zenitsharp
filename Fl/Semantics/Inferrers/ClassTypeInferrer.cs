using Fl.Ast;
using Fl.Semantics.Symbols.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor inferrer, ClassNode node)
        {
            inferrer.SymbolTable.EnterClassScope(node.Name.Value);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(inferrer));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(inferrer));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(inferrer));

            inferrer.SymbolTable.LeaveScope();

            return inferrer.SymbolTable.GetBoundSymbol(node.Name.Value).TypeSymbol;
        }
    }
}
