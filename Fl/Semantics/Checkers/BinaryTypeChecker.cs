// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeCheckerVisitor, BinaryNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, BinaryNode binary)
        {
            var left = binary.Left.Visit(checker);
            var right = binary.Right.Visit(checker);

            if (!left.TypeInfo.Type.IsAssignableFrom(right.TypeInfo.Type))
                throw new System.Exception($"Operator {binary.Operator.Value} cannot be applied on operands of type {left.TypeInfo} and {right.TypeInfo}");

            left.Symbol = null;

            return left;
        }
    }
}
