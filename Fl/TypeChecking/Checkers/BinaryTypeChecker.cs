// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Engine.Symbols.Types;
using Fl.Parser;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecking.Checkers
{
    class BinaryTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBinaryNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstBinaryNode binary)
        {
            Type left = binary.Left.Visit(checker);
            Type right = binary.Right.Visit(checker);

            if (left != right)
                throw new System.Exception($"Operator {binary.Operator.Value} cannot be applied on operands of type {left} and {right}");

            return left;
        }
    }
}
