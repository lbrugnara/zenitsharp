// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics.Exceptions;

namespace Fl.Semantics.Checkers
{
    class ConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, ConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ConstantNode constdec)
        {
            CheckedType lhsType = null;

            foreach (var definition in constdec.Definitions)
            {
                if (lhsType == null)
                    lhsType = new CheckedType(checker.SymbolTable.GetSymbol(definition.Left.Value).TypeInfo);

                // Get the right-hand side operand (a must for a constant)
                var rhsType = definition.Right.Visit(checker);

                if (!lhsType.TypeInfo.Type.IsAssignableFrom(rhsType.TypeInfo.Type))
                    throw new SymbolException($"Cannot assign type {rhsType.TypeInfo} to constant of type {lhsType.TypeInfo}");

            }

            return lhsType;
        }
    }
}
