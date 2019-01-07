// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;
using Fl.Semantics.Types;
using Fl.Semantics;
using Fl.Semantics.Symbols;

namespace Fl.Semantics.Mutability
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
