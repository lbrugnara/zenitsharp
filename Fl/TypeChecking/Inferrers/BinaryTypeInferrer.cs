// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class BinaryTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstBinaryNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstBinaryNode binary)
        {
            var left = binary.Left.Visit(visitor);
            var right = binary.Right.Visit(visitor);

            return visitor.Inferrer.MakeConclusion(left, right);
        }
    }
}
