// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeCheckerVisitor, BinaryNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, BinaryNode binary)
        {
            var left = binary.Left.Visit(checker);
            var right = binary.Right.Visit(checker);

            /*if (!left.TypeSymbol.Type.IsAssignableFrom(right.TypeSymbol.Type))
                throw new System.Exception($"Operator {binary.Operator.Value} cannot be applied on operands of type {left.TypeSymbol} and {right.TypeSymbol}");*/

            left.Symbol = null;

            return left;
        }
    }
}
