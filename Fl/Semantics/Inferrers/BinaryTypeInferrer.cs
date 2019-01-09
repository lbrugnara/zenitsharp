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

            return new InferredType(visitor.Inferrer.FindMostGeneralType(left.TypeInfo, right.TypeInfo));
        }
    }
}
