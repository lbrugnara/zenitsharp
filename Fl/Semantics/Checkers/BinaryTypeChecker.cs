// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBinaryNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstBinaryNode binary)
        {
            var left = binary.Left.Visit(checker);
            var right = binary.Right.Visit(checker);

            if (!left.Type.IsAssignableFrom(right.Type))
                throw new System.Exception($"Operator {binary.Operator.Value} cannot be applied on operands of type {left.Type} and {right.Type}");

            left.Symbol = null;

            return left;
        }
    }
}
