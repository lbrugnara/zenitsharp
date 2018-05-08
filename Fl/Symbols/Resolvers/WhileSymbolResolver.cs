// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class WhileSymbolResolver : INodeVisitor<SymbolResolver, AstWhileNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.SymbolTable.EnterBlock(BlockType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            wnode.Condition.Visit(checker);

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.SymbolTable.LeaveBlock();

            return null;
        }
    }
}
