// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file


using Fl.Ast;

namespace Fl.Symbols.Resolvers
{
    class IfSymbolResolver : INodeVisitor<SymbolResolverVisitor, AstIfNode>
    {
        public void Visit(SymbolResolverVisitor checker, AstIfNode ifnode)
        {
            // Generate the condition and check the result, using exitPoint
            // as the destination if the condition is true
            ifnode.Condition.Visit(checker);            
            
            // Add a new common block for the if's boyd
            checker.SymbolTable.EnterBlock(BlockType.Common, $"if-then-{ifnode.GetHashCode()}");

            // Generate the if's body
            ifnode.Then?.Visit(checker);

            // Leave the if's then block
            checker.SymbolTable.LeaveBlock();

            if (ifnode.Else != null)
            {
                // Add a block for the else's body and generate it, then leave the block
                checker.SymbolTable.EnterBlock(BlockType.Common, $"if-else-{ifnode.GetHashCode()}");
                ifnode.Else.Visit(checker);                
                checker.SymbolTable.LeaveBlock();
            }
        }
    }
}
