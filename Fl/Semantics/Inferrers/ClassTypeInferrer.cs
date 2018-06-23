using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstClassNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AstClassNode node)
        {
            inferrer.SymbolTable.EnterClassScope(node.Name.Value.ToString());

            node.Properties.ForEach(propertyNode => propertyNode.Visit(inferrer));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(inferrer));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(inferrer));

            inferrer.SymbolTable.LeaveScope();

            return new InferredType(inferrer.SymbolTable.Global.GetSymbol(node.Name.Value.ToString()).Type);
        }
    }
}
