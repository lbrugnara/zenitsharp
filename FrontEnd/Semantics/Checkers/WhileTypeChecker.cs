﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Checkers
{
    class WhileTypeChecker : INodeVisitor<TypeCheckerVisitor, WhileNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.SymbolTable.EnterLoopScope($"{wnode.Uid}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(checker);

            if (conditionType.TypeSymbol.BuiltinType != BuiltinType.Bool)
                throw new System.Exception($"For condition needs a {BuiltinType.Bool.GetName()} expression");

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
