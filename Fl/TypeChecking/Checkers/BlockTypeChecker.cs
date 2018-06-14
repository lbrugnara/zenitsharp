// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Symbols.Types;
using Fl.Symbols;

namespace Fl.TypeChecking.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBlockNode, SType>
    {
        public SType Visit(TypeCheckerVisitor checker, AstBlockNode node)
        {
            checker.EnterBlock(ScopeType.Common, $"block-{node.GetHashCode()}");
            foreach (AstNode statement in node.Statements)
            {
                statement.Visit(checker);
            }
            checker.LeaveBlock();
            return Null.Instance;
        }
    }
}
