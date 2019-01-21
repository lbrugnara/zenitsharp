// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class NullCoalescingTypeChecker : INodeVisitor<TypeCheckerVisitor, NullCoalescingNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, NullCoalescingNode nullc)
        {
            var left = nullc.Left.Visit(checker);
            var right = nullc.Right.Visit(checker);

            /*if (!left.TypeSymbol.Type.IsAssignableFrom(right.TypeSymbol.Type))
                throw new System.Exception($"Operator ?? cannot be applied to operands of type {left.TypeSymbol} and {right.TypeSymbol}");*/

            left.Symbol = null;

            return left;
        }
    }
}
