// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Parser.Ast;
using Fl.Symbols;

namespace Fl.TypeChecker.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeChecker, AstBlockNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstBlockNode node)
        {
            checker.EnterBlock(BlockType.Common, $"block-{node.GetHashCode()}");
            foreach (AstNode statement in node.Statements)
            {
                statement.Visit(checker);
            }
            checker.LeaveBlock();
            return null;
        }
    }
}
