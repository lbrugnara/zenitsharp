// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Inferrers
{
    class NullCoalescingTypeInferrer : INodeVisitor<TypeInferrerVisitor, NullCoalescingNode, InferredType>
    {
        public InferredType Visit(TypeInferrerVisitor visitor, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            visitor.Inferrer.FindMostGeneralType(left.TypeInfo, right.TypeInfo);

            return new InferredType(left.TypeInfo);
        }
    }
}
