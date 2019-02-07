// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;
using Zenit.Semantics.Symbols;
using Zenit.Semantics.Types;

namespace Zenit.Semantics.Checkers
{
    class ForTypeChecker : INodeVisitor<TypeCheckerVisitor, ForNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, ForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.SymbolTable.EnterLoopScope($"{fornode.Uid}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Emmit the condition code
            var conditionType = fornode.Condition.Visit(checker);

            if (conditionType.TypeSymbol.BuiltinType != BuiltinType.Bool)
                throw new System.Exception($"For condition needs a {BuiltinType.Bool.GetName()} expression");

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
