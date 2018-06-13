// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Inferrers
{
    class NullCoalescingTypeInferrer : INodeVisitor<TypeInferrerVisitor, AstNullCoalescingNode, Type>
    {
        public Type Visit(TypeInferrerVisitor visitor, AstNullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(visitor);
            var right = nullc.Right.Visit(visitor);

            return visitor.Inferrer.MakeConclusion(left.DataType, right.DataType);
        }
    }
}
