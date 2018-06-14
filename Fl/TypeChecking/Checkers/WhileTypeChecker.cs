// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class WhileTypeChecker : INodeVisitor<TypeCheckerVisitor, AstWhileNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.EnterBlock(ScopeType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var conditionType = wnode.Condition.Visit(checker);

            if (conditionType != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.LeaveBlock();

            return null;
        }
    }
}
