using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassConstantTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassConstantNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor inferrer, ClassConstantNode node)
        {
            // Get the constant symbol
            var constant = inferrer.SymbolTable.GetSymbol(node.Name.Value);

            // Get the inferred type of the right-hand side expression and make the conclusions
            var defInferredType = node.Definition.Visit(inferrer);

            // Use the ClassProperty.Type type in the inference process
            inferrer.Inferrer.InferFromType(constant.Type, defInferredType.Type);

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple constant declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return new InferredType(constant.Type, constant);
        }
    }
}
