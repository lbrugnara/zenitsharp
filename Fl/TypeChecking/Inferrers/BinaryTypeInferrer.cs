// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBinaryNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, AstBinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            // Make conclusions about the types if possible
            return new InferredType(visitor.Inferrer.MakeConclusion(left.Type, right.Type));
        }
    }
}
