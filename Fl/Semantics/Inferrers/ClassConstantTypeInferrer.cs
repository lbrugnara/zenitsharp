using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstClassConstantNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AstClassConstantNode node)
        {
            // TODO: Implement ClassConstant inferrer
            return null;
        }
    }
}
