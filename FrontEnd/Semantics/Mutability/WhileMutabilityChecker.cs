// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class WhileMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, WhileNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, WhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.SymbolTable.EnterLoopScope($"{wnode.Uid}");

            wnode.Condition.Visit(checker);

            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
