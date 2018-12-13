// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, BinaryNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, BinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            visitor.Inferrer.Unify(left.TypeInfo, right.TypeInfo);

            // Make conclusions about the types if possible
            return new InferredType(left.TypeInfo);
        }
    }
}
