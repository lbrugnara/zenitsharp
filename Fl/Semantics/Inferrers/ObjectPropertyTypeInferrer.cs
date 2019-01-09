// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Inferrers
{
    class ObjectPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectPropertyNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ObjectPropertyNode node)
        {
            var rightType = visitor.Visit(node.Value);

            var property = visitor.SymbolTable.Get(node.Name.Value);

            // Get the most general type
            var generalType = visitor.Inferrer.FindMostGeneralType(property.TypeInfo, rightType.TypeInfo);
            
            // Unify the types
            visitor.Inferrer.Unify(generalType, property.TypeInfo, rightType.TypeInfo);

            return new InferredType(property.TypeInfo, property);
        }
    }
}
