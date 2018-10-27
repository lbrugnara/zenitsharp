using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassPropertyNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, ClassPropertyNode node)
        {
            // Get the property symbol
            var property = inferrer.SymbolTable.GetSymbol(node.Name.Value);

            // If the right-hand side is present, get the inferred type and make the conclusions
            var defInferredType = node.Definition?.Visit(inferrer);

            // Use the ClassProperty.Type property in the inference
            if (defInferredType != null)
                inferrer.Inferrer.MakeConclusion(property.Type, defInferredType.Type);

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple property declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new InferredType(property.Type, property);
        }
    }
}
