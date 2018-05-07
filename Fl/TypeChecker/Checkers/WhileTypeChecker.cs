// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Parser.Ast;

namespace Fl.TypeChecker.Checkers
{
    class WhileTypeChecker : INodeVisitor<TypeChecker, AstWhileNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstWhileNode wnode)
        {
            // Generate an eblock instruction for the whole while-block
            checker.EnterBlock(BlockType.Loop, $"while-body-{wnode.GetHashCode()}");

            // Emmit the condition code
            var condition = wnode.Condition.Visit(checker);

            // Emmit the body code
            wnode.Body.Visit(checker);

            // Leave the while-block
            checker.LeaveBlock();

            return null;
        }
    }
}
