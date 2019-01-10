using Fl.Ast;
using Fl.Semantics.Inferrers;
using Fl.Semantics.Symbols;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class ClassPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ClassPropertyNode, ITypeSymbol>
    {
        public ITypeSymbol Visit(TypeInferrerVisitor inferrer, ClassPropertyNode node)
        {
            // Get the property symbol
            var property = inferrer.SymbolTable.GetBoundSymbol(node.Name.Value);

            // If the right-hand side is present, get the inferred type and make the conclusions
            var defIValueSymbol = node.Definition?.Visit(inferrer);

            // Use the ClassProperty.Type property in the inference
            if (defIValueSymbol != null)
                inferrer.Inferrer.FindMostGeneralType(property.TypeSymbol, defIValueSymbol);

            // TODO: By now return the ClassProperty, as the result does not need to be used,
            // but if in the future we support multiple property declaration, we need to review
            // this, as we would want the ClassProperty.Type type
            return property.TypeSymbol;
        }
    }
}
