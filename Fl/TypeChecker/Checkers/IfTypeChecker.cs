// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Symbols;
using Fl.Ast;

namespace Fl.TypeChecker.Checkers
{
    class IfTypeChecker : INodeVisitor<TypeChecker, AstIfNode, Symbol>
    {
        public Symbol Visit(TypeChecker checker, AstIfNode ifnode)
        {
            var condition = ifnode.Condition.Visit(checker);            

            // Add a new common block for the if's boyd
            checker.EnterBlock(BlockType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.LeaveBlock();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                checker.EnterBlock(BlockType.Common, $"if-else-{ifnode.GetHashCode()}");
                ifnode.Else.Visit(checker);                
                checker.LeaveBlock();
            }

            return null;
        }
    }
}
