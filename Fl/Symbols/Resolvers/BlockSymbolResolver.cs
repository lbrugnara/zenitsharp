// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class BlockSymbolResolver : INodeVisitor<SymbolResolver, AstBlockNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstBlockNode node)
        {
            checker.SymbolTable.EnterBlock(BlockType.Common, $"block-{node.GetHashCode()}");

            foreach (AstNode statement in node.Statements)
                statement.Visit(checker);

            checker.SymbolTable.LeaveBlock();
            return null;
        }
    }
}
