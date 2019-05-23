// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Checkers
{
    class ConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, ConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ConstantNode constdec)
        {
            CheckedType lhsType = null;

            foreach (var definition in constdec.Definitions)
            {
                if (lhsType == null)
                    lhsType = new CheckedType(checker.SymbolTable.GetVariableSymbol(definition.Left.Value).TypeSymbol);

                // Get the right-hand side operand (a must for a constant)
                var rhsType = definition.Right.Visit(checker);

                /*if (!lhsType.TypeSymbol.Type.IsAssignableFrom(rhsType.TypeSymbol.Type))
                    throw new SymbolException($"Cannot assign type {rhsType.TypeSymbol} to constant of type {lhsType.TypeSymbol}");*/

            }

            return lhsType;
        }
    }
}
