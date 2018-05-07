// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;

using Fl.Parser.Ast;

namespace Fl.Symbols.Resolvers
{
    class ForSymbolResolver : INodeVisitor<SymbolResolver, AstForNode, Symbol>
    {
        public Symbol Visit(SymbolResolver checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.EnterBlock(BlockType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Emmit the condition code
            var condition = fornode.Condition.Visit(checker);

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.LeaveBlock();

            return null;
        }
    }
}
