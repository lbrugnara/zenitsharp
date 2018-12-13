using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, ClassNode node)
        {
            inferrer.SymbolTable.EnterClassScope(node.Name.Value);

            node.Properties.ForEach(propertyNode => propertyNode.Visit(inferrer));

            node.Constants.ForEach(constantInfo => constantInfo.Visit(inferrer));

            node.Methods.ForEach(methodInfo => methodInfo.Visit(inferrer));

            inferrer.SymbolTable.LeaveScope();

            return new InferredType(inferrer.SymbolTable.GetSymbol(node.Name.Value).TypeInfo);
        }
    }
}
