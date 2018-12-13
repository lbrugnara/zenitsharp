// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class NullCoalescingTypeChecker : INodeVisitor<TypeCheckerVisitor, NullCoalescingNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(checker);
            var right = nullc.Right.Visit(checker);

            if (!left.TypeInfo.Type.IsAssignableFrom(right.TypeInfo.Type))
                throw new System.Exception($"Operator ?? cannot be applied to operands of type {left.TypeInfo} and {right.TypeInfo}");

            left.Symbol = null;

            return left;
        }
    }
}
