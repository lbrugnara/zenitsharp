// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class IfMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, IfNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, IfNode ifnode)
        {
            ifnode.Condition.Visit(checker);

            // Add a new common block for the if's boyd
            checker.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.SymbolTable.LeaveScope();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body, check it, then leave the block
                checker.SymbolTable.EnterBlockScope($"{ifnode.Uid}");

                ifnode.Else.Visit(checker);

                checker.SymbolTable.LeaveScope();
            }

            return null;
        }
    }
}
