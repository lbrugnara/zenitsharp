// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols.Types;

namespace Zenit.Semantics.Inferrers
{
    class NullCoalescingTypeInferrer : INodeVisitor<TypeInferrerVisitor, NullCoalescingNode, IType>
    {
        public IType Visit(TypeInferrerVisitor visitor, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            return visitor.Inferrer.FindMostGeneralType(left, right);
        }
    }
}
