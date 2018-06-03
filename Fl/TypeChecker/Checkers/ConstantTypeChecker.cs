// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Lang.Types;

namespace Fl.TypeChecker.Checkers
{
    class ConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, AstConstantNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstConstantNode constdec)
        {
            // Get the constant's type
            var lhsType = TypeHelper.FromToken(constdec.Type);

            foreach (var declaration in constdec.Constants)
            {
                // Get the right-hand side operand (a must for a constant)
                var rhsType = declaration.Item2.Visit(checker);

                if (lhsType != rhsType)
                    throw new System.Exception($"Cannot convert {rhsType} to {lhsType}");

            }

            return lhsType;
        }
    }
}
