// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBinaryNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstBinaryNode binary)
        {
            SType left = binary.Left.Visit(checker);
            SType right = binary.Right.Visit(checker);

            if (left != right)
                throw new System.Exception($"Operator {binary.Operator.Value} cannot be applied on operands of type {left} and {right}");

            return left;
        }
    }
}
