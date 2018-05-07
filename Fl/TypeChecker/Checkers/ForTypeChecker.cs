// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser.Ast;

namespace Fl.TypeChecker.Checkers
{
    class ForTypeChecker : INodeVisitor<TypeChecker, AstForNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstForNode fornode)
        {
            // Create a new block to contain the for's initialization
            checker.EnterBlock(BlockType.Loop, $"for-{fornode.GetHashCode()}");

            // Initialize the for-block
            fornode.Init.Visit(checker);

            // Generate the loop block (for's body) with the entry and exit points
            //checker.EnterBlock(BlockType.Loop, $"for-body-{fornode.GetHashCode()}");

            // Emmit the condition code
            var condition = fornode.Condition.Visit(checker);

            // Emmit the body code
            fornode.Body.Visit(checker);

            // Emmit the for's increment part
            fornode.Increment.Visit(checker);

            // Leave the for
            checker.LeaveBlock();

            // Leave the for-block
            //checker.LeaveBlock();

            return null;
        }
    }
}
