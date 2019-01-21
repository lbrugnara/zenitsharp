// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Ast;

namespace Fl.Semantics.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeCheckerVisitor, BlockNode, CheckedType>
    {
        public CheckedType Visit(TypeCheckerVisitor checker, BlockNode node)
        {
            checker.SymbolTable.EnterBlockScope($"{node.Uid}");

            foreach (Node statement in node.Statements)
                statement.Visit(checker);

            checker.SymbolTable.LeaveScope();

            return null;
        }
    }
}
