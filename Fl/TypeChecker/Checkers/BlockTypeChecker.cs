// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;
using Fl.Lang.Types;
using Fl.Symbols;

namespace Fl.TypeChecker.Checkers
{
    class BlockTypeChecker : INodeVisitor<TypeCheckerVisitor, AstBlockNode, Type>
    {
        public Type Visit(TypeCheckerVisitor checker, AstBlockNode node)
        {
            checker.EnterBlock(BlockType.Common, $"block-{node.GetHashCode()}");
            foreach (AstNode statement in node.Statements)
            {
                statement.Visit(checker);
            }
            checker.LeaveBlock();
            return Null.Instance;
        }
    }
}
