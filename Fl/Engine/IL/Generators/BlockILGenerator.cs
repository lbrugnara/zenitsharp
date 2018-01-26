// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

using Fl.Engine.IL.Instructions.Operands;
using Fl.Parser.Ast;

namespace Fl.Engine.IL.Generators
{
    class BlockILGenerator : INodeVisitor<ILGenerator, AstBlockNode, Operand>
    {
        public Operand Visit(ILGenerator generator, AstBlockNode node)
        {
            generator.EnterBlock(BlockType.Common);
            foreach (AstNode statement in node.Statements)
            {
                statement.Exec(generator);
            }
            generator.LeaveBlock();
            return null;
        }
    }
}
