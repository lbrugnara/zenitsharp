using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstClassPropertyNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, AstClassPropertyNode node)
        {
            // TODO: Implement ClassProperty inferrer
            return null;
        }
    }
}
