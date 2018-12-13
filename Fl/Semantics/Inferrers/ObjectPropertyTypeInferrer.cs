// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Inferrers
{
    class ObjectPropertyTypeInferrer : INodeVisitor<TypeInferrerVisitor, ObjectPropertyNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, ObjectPropertyNode node)
        {
            var inferredType = visitor.Visit(node.Value);
            inferredType.Symbol = visitor.SymbolTable.GetSymbol(node.Name.Value);
            return inferredType;
        }
    }
}
