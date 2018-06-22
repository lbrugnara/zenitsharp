﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Semantics;
using Fl.Ast;
using Fl.Semantics.Types;

namespace Fl.Semantics.Checkers
{
    class ConstantTypeChecker : INodeVisitor<TypeCheckerVisitor, AstConstantNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, AstConstantNode constdec)
        {
            CheckedType lhsType = null;

            foreach (var declaration in constdec.Constants)
            {
                if (lhsType == null)
                    lhsType = new CheckedType(checker.SymbolTable.GetSymbol(declaration.Item1.Value.ToString()).Type);

                // Get the right-hand side operand (a must for a constant)
                var rhsType = declaration.Item2.Visit(checker);

                if (!lhsType.Type.IsAssignableFrom(rhsType.Type))
                    throw new System.Exception($"Cannot convert {rhsType.Type} to {lhsType.Type}");

            }

            return lhsType;
        }
    }
}