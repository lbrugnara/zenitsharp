// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Zenit.Ast;

namespace Zenit.Semantics.Mutability
{
    class BlockMutabilityChecker : INodeVisitor<MutabilityCheckerVisitor, BlockNode, MutabilityCheckResult>
    {
        public MutabilityCheckResult Visit(MutabilityCheckerVisitor checker, BlockNode node)
        {
            checker.SymbolTable.EnterBlockScope($"{node.Uid}");

            foreach (Node statement in node.Statements)
                statement.Visit(checker);

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
