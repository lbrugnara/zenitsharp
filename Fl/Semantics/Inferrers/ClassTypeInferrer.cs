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
            // TODO: Implement Class inferrer
            return new InferredType(inferrer.SymbolTable.Global.GetSymbol(node.Name.Value.ToString()).Type);
        }
    }
}
