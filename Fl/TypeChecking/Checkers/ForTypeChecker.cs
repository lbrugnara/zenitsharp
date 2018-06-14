// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;
using Fl.Symbols.Types;

namespace Fl.TypeChecking.Checkers
{
    class ForTypeChecker : INodeVisitor<TypeCheckerVisitor, AstForNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.EnterBlock(ScopeType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(checker);

            if (conditionType != Bool.Instance)
                throw new System.Exception($"For condition needs a {Bool.Instance} expression");

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.LeaveBlock();

            return Null.Instance;
        }
    }
}
